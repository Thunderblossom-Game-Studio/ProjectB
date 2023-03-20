using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JE.DamageSystem;

public class ZoneCollision : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerStay(Collider other)
    {
        IDamageable damager = other.GetComponent<IDamageable>();
        if ((playerLayer.value & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
        {
            Debug.Log("TAKING DAMAGE IN THE ZONE");
            damager.ReduceHealth(1);
        }
    }
}
