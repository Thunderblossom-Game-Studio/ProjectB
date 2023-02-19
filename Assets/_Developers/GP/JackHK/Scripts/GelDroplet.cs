using System.Collections;
using System.Collections.Generic;
using JE.General;
using UnityEngine;

public class GelDroplet : MonoBehaviour
{
    public GelSystem _gelSystem;

    private void Awake()
    {
        if (_gelSystem == null)
        {
            _gelSystem = FindObjectOfType<GelSystem>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_gelSystem._groundLayerName.ContainsLayer(other.gameObject.layer)) return;
        _gelSystem.CreateSplatter(gameObject, other.transform);
        DestroyDroplet();
    }
        
    public void DestroyDroplet()
    {
        if (_gelSystem._isObjectPooling == true)
        {
            //_gelSystem._splatterObjectPool.Return(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
