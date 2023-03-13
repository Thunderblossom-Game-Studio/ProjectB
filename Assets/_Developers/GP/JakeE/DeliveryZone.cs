using System;
using System.Collections;
using System.Collections.Generic;
using JE.General;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class DeliveryZone : MonoBehaviour
{
    [SerializeField] private UnityEvent _onDeliver;

    private void OnTriggerEnter(Collider objectCollider)
    {
        GameObject baseObject = objectCollider.gameObject.transform.root.gameObject;
        if (!baseObject.TryGetComponent(out PackageSystem packageSystem)) return;
        if (packageSystem.PackageAmount < 1) return;
        
        _onDeliver?.Invoke();
        packageSystem.DeliverPackages();
    }
}
