using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVFX : MonoBehaviour
{
    [SerializeField] private GameObject _vfxPrefab;
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private float _destroyDelay;

    public void Spawn()
    {
        GameObject vfx = Instantiate(_vfxPrefab, transform.position + _positionOffset, Quaternion.identity);
        Destroy(vfx, _destroyDelay);
    }

    private void OnDrawGizmos()
    {
        Vector3 cubeSize = new Vector3(0.2f, 0.2f, 0.2f);
        Gizmos.DrawWireCube(transform.position + _positionOffset, cubeSize);
    }
}
