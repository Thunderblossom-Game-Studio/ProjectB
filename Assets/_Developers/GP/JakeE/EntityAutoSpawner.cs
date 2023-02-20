using System;
using System.Collections;
using System.Collections.Generic;
using JE.Utilities;
using UnityEngine;

[RequireComponent(typeof(EntitySpawner))]
public class EntityAutoSpawner : MonoBehaviour
{
    enum Status
    {
        Enabled,
        Disabled
    }

    [Header("Settings")] 
    [SerializeField] Status _spawnerStatus;
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _spawnMaximum;

    private EntitySpawner _spawnerEntity;
    private Timer _spawnerTimer;

    private void Start()
    {
        _spawnerEntity = GetComponent<EntitySpawner>();
        _spawnerTimer = new Timer(_spawnRate, _spawnerEntity.Spawn);
    }
    private void Update() => Spawn();

    private void Spawn()
    {
        if (_spawnerStatus != Status.Enabled) return;
        if (_spawnerEntity.SpawnedObjects.Count >= _spawnMaximum) return;
        _spawnerTimer.Tick(Time.deltaTime);
    }
}
