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

    private void OnTriggerEnter(Collider objectCollider)
    {
        if (!_collideLayer.ContainsLayer(objectCollider.gameObject.layer)) return;
        if (_teleportDestinations.Count < 1) return;
        StartCoroutine(Teleport(objectCollider.gameObject));
    }

    private IEnumerator Teleport(GameObject collideObject)
    {
        yield return new WaitForSeconds(_teleportDelay);
        Teleporter randomTeleporter = GetRandomTeleporter();
        Rigidbody objectBody = collideObject.GetComponent<Rigidbody>();
        collideObject.transform.position = randomTeleporter.TeleportLocation;
        collideObject.transform.forward = randomTeleporter.transform.forward;
        objectBody.velocity = randomTeleporter.transform.forward * Mathf.Abs(SumVelocity(objectBody.velocity));
    }

    private Teleporter GetRandomTeleporter()
    {
        int index = Random.Range(0, _teleportDestinations.Count - 1);
        return _teleportDestinations[index];
    }

    private float SumVelocity(Vector3 velocity) => velocity.x + velocity.y + velocity.z;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + _teleportLocation, new Vector3(0.3f, 0.3f, 0.3f));
    }
}
