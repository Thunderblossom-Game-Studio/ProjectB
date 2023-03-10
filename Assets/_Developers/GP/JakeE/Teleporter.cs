using System;
using System.Collections;
using System.Collections.Generic;
using JE.General;
using UnityEngine;
using Random = UnityEngine.Random;

public class Teleporter : MonoBehaviour
{
    #region GET & SET
    public Vector3 TeleportLocation => transform.position + _teleportLocation;
    #endregion

    [SerializeField] private LayerMask _collideLayer;
    [SerializeField] private Vector3 _teleportLocation;
    [SerializeField] [Range(0, 10)] private float _teleportDelay;
    [SerializeField] private List<Teleporter> _teleportDestinations;

    private IEnumerator OnTriggerEnter(Collider objectCollider)
    {
        if (!_collideLayer.ContainsLayer(objectCollider.gameObject.layer)) 
            yield break;
        
        if (_teleportDestinations.Count < 1) 
            yield break;
        
        yield return new WaitForSeconds(_teleportDelay);
        yield return Teleport(objectCollider.gameObject);
    }

    private IEnumerator Teleport(GameObject collideObject)
    {
        yield return new WaitForSeconds(_teleportDelay);
        Teleporter randomTeleporter = GetRandomTeleporter();
        
        Transform parentObject = collideObject.transform.root;
        parentObject.position = randomTeleporter.TeleportLocation;
        parentObject.forward = randomTeleporter.transform.forward;
        
        Rigidbody objectBody = collideObject.GetComponentInParent<Rigidbody>();
        
        if (objectBody)
            objectBody.velocity = randomTeleporter.transform.forward * objectBody.velocity.magnitude;
    }

    private Teleporter GetRandomTeleporter()
    {
        int index = Random.Range(0, _teleportDestinations.Count - 1);
        return _teleportDestinations[index];
    }

    private void OnDrawGizmos() =>
        Gizmos.DrawWireCube(transform.position + _teleportLocation, new Vector3(0.3f, 0.3f, 0.3f));
}
