using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArcadeCarImpact : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collision> _OnHit;

    private void OnCollisionEnter(Collision collision)
    {
        _OnHit.Invoke(collision);
    }
}
