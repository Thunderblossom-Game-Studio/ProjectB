using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnObject;
    [SerializeField] private Vector3 _spawnOffset;
    [SerializeField] private float _destroyDuration;
    
    public void Spawn()
    {
        GameObject spawnObject = Instantiate(_spawnObject, transform.position + _spawnOffset, Quaternion.identity);
        Destroy(spawnObject, _destroyDuration);
    }
}
