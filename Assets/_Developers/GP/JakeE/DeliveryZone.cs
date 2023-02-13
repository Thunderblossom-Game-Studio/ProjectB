using System;
using System.Collections;
using System.Collections.Generic;
using JE.General;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeliveryZone : MonoBehaviour
{
    [SerializeField] private LayerMask _detectedLayers;
    public static Action OnDeliver;
    
    private void OnTriggerEnter(Collider objectCollider)
    {
        if (!_detectedLayers.ContainsLayer(objectCollider.gameObject.layer)) return;
        OnDeliver?.Invoke();
    }
}
