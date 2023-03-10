using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntitySpawner : MonoBehaviour
{
    #region GET & SET
    public List<SpawnableObject> SpawnedObjects => _spawnedObjects;

    #endregion
    
    private enum SpawnMode
    {
        Random,
        All
    }

    [SerializeField] private bool _isLocal;
    [SerializeField] private SpawnMode _spawnMode;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Color _spawnZoneColor;
    [SerializeField] private List<Zone> _spawnZones;
    [SerializeField] private List<ObjectType> _objectTypes;
    [SerializeField] private List<SpawnableObject> _spawnedObjects = new List<SpawnableObject>();

    public void Spawn()
    {
        if (IsSetup()) return;
        switch (_spawnMode)
        {
            case SpawnMode.Random:
                SpawnRandom();
                break;
            case SpawnMode.All:
                SpawnAll();
                break;
        }
    }

    private void SpawnRandom()
    {
        Zone randomZone = _spawnZones[Random.Range(0, _spawnZones.Count)];
        Vector3 randomPosition = GetRandomPosition(randomZone);
        Vector3 spawnPosition = ConvertToGroundPosition(randomZone, randomPosition);
        GameObject spawnedObject = Instantiate(GetObject().Object, spawnPosition, Quaternion.identity);
        spawnedObject.AddComponent<SpawnableObject>().Initialise(this);
    }

    private void SpawnAll()
    {
        foreach (GameObject spawnedObject in from spawnZone in _spawnZones let randomPosition 
                     = GetRandomPosition(spawnZone) select ConvertToGroundPosition(spawnZone, randomPosition) 
                 into spawnPosition select Instantiate(GetObject().Object, spawnPosition, Quaternion.identity))
        {
            spawnedObject.AddComponent<SpawnableObject>().Initialise(this);
        }
    }
    
    private Vector3 GetRandomPosition(Zone randomZone)
    {
        if (!_isLocal)
            return new Vector3(randomZone.Position.x + Random.Range(-randomZone.Size.x / 2, randomZone.Size.x / 2),
            randomZone.Position.y + Random.Range(-randomZone.Size.y / 2, randomZone.Size.y / 2),
            randomZone.Position.z + Random.Range(-randomZone.Size.z / 2, randomZone.Size.z / 2));
        
        return new Vector3((randomZone.Position.x + transform.position.x) + Random.Range(-randomZone.Size.x / 2, randomZone.Size.x / 2),
            (randomZone.Position.y + transform.position.y) + Random.Range(-randomZone.Size.y / 2, randomZone.Size.y / 2),
            (randomZone.Position.z + transform.position.z) + Random.Range(-randomZone.Size.z / 2, randomZone.Size.z / 2));
    }

    private ObjectType GetObject()
    {
        float totalWeight = 0;
        foreach (var objectType in _objectTypes) { totalWeight += objectType.Probability; }
        float randomWeight = Random.Range(0, totalWeight);
        List<float> weightTable = GetWeightedTable();
        foreach (float item in weightTable)
        {
            if (randomWeight <= item)
            {
                int index = weightTable.IndexOf(item);
                return _objectTypes[index];
            }
        }
        return null;
    }

    private List<float> GetWeightedTable()
    {
        float currentWeight = 0;
        List<float> weightList = new List<float>();
        foreach (ObjectType objectWeight in _objectTypes)
        {
            currentWeight += objectWeight.Probability;
            weightList.Add(currentWeight);
        }
        return weightList;
    }

    private Vector3 ConvertToGroundPosition(Zone selectedZone, Vector3 randomPosition)
    {
        return Physics.Raycast(new Vector3(randomPosition.x, selectedZone.Position.y + selectedZone.Size.y / 2, randomPosition.z),
            Vector3.down, out var raycastHit, selectedZone.Size.y * 2, _groundLayer) ? raycastHit.point : randomPosition;
    }

    private void OnDrawGizmos()
    {
        if (_spawnZones.Count <= 0) return;
        Gizmos.color = _spawnZoneColor;

        if (!_isLocal)
        {
            foreach (Zone spawnZone in _spawnZones)
            {
                Gizmos.DrawWireCube(spawnZone.Position, spawnZone.Size);
            }
        }

        else
        {
            foreach (Zone spawnZone in _spawnZones)
            {
                Gizmos.DrawWireCube(spawnZone.Position + transform.position, spawnZone.Size);
            }
        }
        
    }
    private bool IsSetup() => _spawnZones.Count <= 0 || _objectTypes.Count <= 0;
}


[Serializable]
public struct Zone
{
    #region Get & Set
    public Vector3 Position => _zonePosition;
    public Vector3 Size => _zoneSize;
    #endregion
    [SerializeField] private Vector3 _zonePosition;
    [SerializeField] private Vector3 _zoneSize;
}

[Serializable]
public class ObjectType
{
    #region Get & Set
    public GameObject Object => _spawnObject;
    public float Probability => _spawnChance;
    #endregion
    [SerializeField] private GameObject _spawnObject;
    [SerializeField] private float _spawnChance;
}
