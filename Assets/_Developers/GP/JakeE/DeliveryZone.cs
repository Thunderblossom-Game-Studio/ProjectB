using System;
using System.Collections;
using System.Collections.Generic;
using JE.General;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeliveryZone : MonoBehaviour
{
    [SerializeField] private LayerMask _detectedLayers;

    private void OnTriggerEnter(Collider objectCollider)
    {
        if (!_detectedLayers.ContainsLayer(objectCollider.gameObject.layer)) return;
        if (!objectCollider.TryGetComponent(out PackageSystem packageSystem)) return;
        packageSystem.DeliverPackages();
    }
}
