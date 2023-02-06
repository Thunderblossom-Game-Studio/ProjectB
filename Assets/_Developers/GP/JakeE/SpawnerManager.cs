using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerManager : Singleton<SpawnerManager>
{
    [SerializeField] private List<EntitySpawner> _entitySpawners = new List<EntitySpawner>();
    public EntitySpawner GetSpawner(int index) => _entitySpawners[index];
}

