using System.Collections;
using JE.General;
using Pelumi.Juicer;
using Unity.VisualScripting;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    [SerializeField] private float _destroyTime = 9f;
    [SerializeField] private float _fadeTime = 3f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _objectMass = 0.2f;
    
    private Renderer _objectRenderer;
    private Rigidbody _objectBody;
    private Collider _currentCollider;
    private Transform parentObject;

    private void Awake()
    {
        parentObject = transform.parent;

        _objectBody = parentObject.GetComponent<Rigidbody>();
        _objectRenderer = parentObject.GetComponent<Renderer>();

        if (!_objectBody)
            _objectBody = parentObject.AddComponent<Rigidbody>();
        
        if (!_objectRenderer) 
            _objectRenderer = parentObject.AddComponent<Renderer>();

        _objectBody.isKinematic = true;
        _objectBody.mass = _objectMass;

        _currentCollider = GetComponent<Collider>();
    }
    
    private IEnumerator OnTriggerEnter(Collider collideObject)
    {
        if (!_layerMask.ContainsLayer(collideObject.gameObject.layer))
            yield break;

        _objectBody.isKinematic = false;
        _currentCollider.enabled = false;

        yield return FadeObject();
    }

    private IEnumerator FadeObject()
    {
        yield return new WaitForSeconds(_destroyTime);
        yield return Juicer.FadeOutMaterial(_objectRenderer.material, _fadeTime, 
            () => parentObject.gameObject.SetActive(false));
    }
}
