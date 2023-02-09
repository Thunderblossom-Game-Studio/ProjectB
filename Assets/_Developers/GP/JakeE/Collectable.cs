using System;
using System.Collections;
using System.Collections.Generic;
using JE.General;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Collectable : MonoBehaviour
{
    [SerializeField] private CollectableType _collectableType;
    [SerializeField] private LayerMask _collideLayers;
    [SerializeField] private bool _destroyOnCollide;
    [SerializeField] private UnityEvent _onCollect;
    
    private void OnTriggerEnter(Collider objectCollider)
    {
        if (!_collideLayers.ContainsLayer(objectCollider.gameObject.layer)) return;
        CollectableManager.Instance.GetCollectable(_collectableType)?.Invoke(objectCollider.gameObject);
        _onCollect.Invoke();
        if (!_destroyOnCollide) return;
        CollectableManager.Instance.DestroyCollectable(this);
    }

    public void SetType(CollectableType collectableType)
    {
        _collectableType = collectableType;
    }
}
