using System;
using System.Collections;
using System.Collections.Generic;
using JE.General;
using UnityEngine;
using Random = UnityEngine.Random;

public class Teleporter : MonoBehaviour
{
    #region GET & SET
    public Vector3 TeleportOffset => 
        transform.position + transform.rotation * _teleportOffset;
    #endregion
    
    public enum TeleportDirection
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        FORWARD,
        BACKWARDS
    }
    
    [SerializeField] private LayerMask _collideLayer;
    [SerializeField] private Vector3 _teleportOffset;
    [SerializeField] private float _teleportDelay;
    
    [SerializeField] private float _teleportTimeLimit = 1;
    [SerializeField] private float _teleportBoost = 1;
    [SerializeField] private TeleportDirection _teleportDirection;

    [SerializeField] private List<Teleporter> _teleportDestinations;

    private readonly List<GameObject> _teleportedObjects = new List<GameObject>();

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
        if (_teleportedObjects.Contains(collideObject))
            yield break;
        
        yield return new WaitForSeconds(_teleportDelay);
        Teleporter randomTeleporter = GetRandomTeleporter();
        
        Transform parentObject = collideObject.transform.root;
        parentObject.position = randomTeleporter.TeleportOffset;
        parentObject.forward = GetDirection(_teleportDirection, randomTeleporter.transform);
        
        Rigidbody objectBody = collideObject.GetComponentInParent<Rigidbody>();
        
        if (objectBody)
            objectBody.velocity = 
                GetDirection(_teleportDirection, randomTeleporter.transform) 
                * (objectBody.velocity.magnitude * _teleportBoost);
        
        _teleportedObjects.Add(collideObject);
        yield return new WaitForSeconds(_teleportTimeLimit);
        _teleportedObjects.Remove(collideObject);
    }

    public Vector3 GetDirection(TeleportDirection teleportDirection, Transform randomTeleporter)
    {
        switch (teleportDirection)
        {
            case TeleportDirection.UP:
                return randomTeleporter.up;
            case TeleportDirection.DOWN:
                return -randomTeleporter.up;
            case TeleportDirection.LEFT:
                return -randomTeleporter.right;
            case TeleportDirection.RIGHT:
                return randomTeleporter.right;
            case TeleportDirection.FORWARD:
                return randomTeleporter.forward;
            case TeleportDirection.BACKWARDS:
                return -randomTeleporter.forward;
        }
        return Vector3.zero;
    }

    private Teleporter GetRandomTeleporter()
    {
        int index = Random.Range(0, _teleportDestinations.Count - 1);
        return _teleportDestinations[index];
    }

    private void OnDrawGizmos()
    {
        float gizmoSize = 0.3f;
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.DrawWireCube
            (_teleportOffset, 
                new Vector3(gizmoSize, gizmoSize, gizmoSize));
    }
}
