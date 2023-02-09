using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerManager : Singleton<SpawnerManager>
{
    [SerializeField] private float _spawnDelay;
    [SerializeField] private List<EntitySpawner> _entitySpawners = new List<EntitySpawner>();
    public EntitySpawner GetSpawner(int index) => _entitySpawners[index];


    private void Spawning()
    {
        GetSpawner(0).Spawn();
    }
    
    private void Start()
    {
        InvokeRepeating(nameof(Spawning), 0, _spawnDelay);
    }
}

