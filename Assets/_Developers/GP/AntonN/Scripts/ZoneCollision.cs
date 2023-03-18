using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCollision : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask safeLayer;
    [SerializeField] private LayerMask dangerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            Debug.Log("Collision with player");
        }

    }
}
