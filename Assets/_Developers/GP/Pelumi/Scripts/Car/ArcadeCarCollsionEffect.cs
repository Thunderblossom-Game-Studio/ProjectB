using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeCarCollsionEffect : MonoBehaviour
{
    [SerializeField] private float _hitVelocityThresold;
    [SerializeField] private GameObject _hitEffect;

    public void OnHit(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < _hitVelocityThresold) return;
        Instantiate(_hitEffect, collision.GetContact(0).point, Quaternion.identity);
    }
}
