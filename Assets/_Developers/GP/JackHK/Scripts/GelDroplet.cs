using System.Collections;
using System.Collections.Generic;
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
        if (other.gameObject.layer == LayerMask.NameToLayer(_gelSystem._groundLayerName))
        {
            _gelSystem.CreateSplatter(gameObject, other.transform);
            DestroyDroplet();
        }
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
