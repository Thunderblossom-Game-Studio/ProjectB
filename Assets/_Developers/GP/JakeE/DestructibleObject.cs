using System;
using System.Collections;
using System.Collections.Generic;
using JE.General;
using Pelumi.Juicer;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private List<Renderer> objectParts;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private float _fadeOutTime = 3f;
    [SerializeField] private float _destroyTime = 9f;

    [SerializeField] private float _additionalExplosionForce;
    [SerializeField] private float _additionalExplosionRadius;
    [SerializeField] private float _additionalUpwardsModifier;
    [SerializeField] private float _objectMass = 0.2f;

    private Collider _currentCollider;
    
    private void Start()
    {
        foreach (Transform child in transform)
        {
            Rigidbody childBody = child.GetComponent<Rigidbody>();
            Renderer childRenderer = child.GetComponent<Renderer>();

            if (!childBody)
                childBody = child.AddComponent<Rigidbody>();

            if (!childRenderer)
                childRenderer = child.AddComponent<Renderer>();

            objectParts.Add(childRenderer);
            childBody.mass = _objectMass;
            childBody.isKinematic = true;
        }
        _currentCollider = GetComponent<Collider>();
    }

    private IEnumerator OnTriggerEnter(Collider collideObject)
    {
        float carSpeed = 0;
        if (!_layerMask.ContainsLayer(collideObject.gameObject.layer))
            yield break;

        GameObject parentObject = collideObject.transform.parent.gameObject;
        
        if (parentObject.TryGetComponent(out Rigidbody objectBody))
            carSpeed = objectBody.velocity.magnitude;

        _currentCollider.enabled = false;
        yield return Explode(carSpeed);
    }

    private IEnumerator Explode(float magnitude)
    {
        foreach (Renderer objectPart in objectParts)
        {
            Rigidbody partBody = objectPart.GetComponent<Rigidbody>();

            partBody.isKinematic = false;
            partBody.AddExplosionForce
                (_additionalExplosionForce + magnitude, transform.position, 
                _additionalExplosionRadius + magnitude,
                _additionalUpwardsModifier + magnitude);
        }
        yield return FadePieces();
    }

    private IEnumerator FadePieces()
    {
        yield return new WaitForSeconds(_destroyTime);
        yield return Juicer.FadeOutAll(objectParts, _fadeOutTime, DisableParts);
    }

    private void DisableParts()
    {
        foreach (Renderer objectPart in objectParts)
            objectPart.gameObject.SetActive(false);
    }
}
