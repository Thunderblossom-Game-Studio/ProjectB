using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{
    [SerializeField] private EntitySpawner _entitySpawner;
    
    public void Initialise(EntitySpawner entitySpawner)
    {
        _entitySpawner = entitySpawner;
        _entitySpawner.SpawnedObjects.Add(this);
    }
    
    public void DestroyObject()
    {
        _entitySpawner.SpawnedObjects.Remove(this);
        Destroy(gameObject);
    }
}
