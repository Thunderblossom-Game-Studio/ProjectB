using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCollision : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if ((playerLayer.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            Debug.Log("Collision with player");

        }
    }
}
