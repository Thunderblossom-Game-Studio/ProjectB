using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawner : MonoBehaviour
{
    enum Status
    {
        Enabled,
        Disabled
    }
    
    [SerializeField] private Status _spawnerStatus;
    [SerializeField] private EntitySpawner _spawnerEntity;

    [Header("Settings")] 
    [SerializeField] private float _spawnRate;
    [SerializeField] private float _spawnMaximum;

    private void Spawn()
    {
        
    }

    private IEnumerator SpawnRoutine()
    {


        yield return null;
    }

}
