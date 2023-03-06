using System;
using System.Collections;
using System.Collections.Generic;
using JE.General;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class DeliveryZone : MonoBehaviour
{
    [SerializeField] private LayerMask _detectedLayers;
    [SerializeField] private UnityEvent _onDeliver;

    private void OnTriggerEnter(Collider objectCollider)
    {
        if (!_detectedLayers.ContainsLayer(objectCollider.gameObject.layer)) return;
        if (!objectCollider.TryGetComponent(out PackageSystem packageSystem)) return;
        if (packageSystem.PackageAmount < 1) return;
        
        _onDeliver?.Invoke();
        packageSystem.DeliverPackages();
    }
}
