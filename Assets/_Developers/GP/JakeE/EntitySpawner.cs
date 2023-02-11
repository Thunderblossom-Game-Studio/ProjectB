using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntitySpawner : MonoBehaviour
{
    #region GET & SET
    public List<SpawnableObject> SpawnedObjects => _spawnedObjects;

    #endregion
    
    
    [SerializeField] private Color _spawnZoneColor;
    [SerializeField] private List<Zone> _spawnZones;
    [SerializeField] private List<ObjectType> _objectTypes;
    [SerializeField] private List<SpawnableObject> _spawnedObjects = new List<SpawnableObject>();

    public void Spawn()
    {
        if (_spawnZones.Count <= 0 || _objectTypes.Count <= 0) return;
        Zone randomZone = _spawnZones[Random.Range(0, _spawnZones.Count)];
        Vector3 randomPosition = GetRandomPosition(randomZone);
        GameObject spawnedObject = Instantiate(GetObject().Object, randomPosition, Quaternion.identity);
        spawnedObject.AddComponent<SpawnableObject>().Initialise(this);
    }

    private Vector3 GetRandomPosition(Zone randomZone)
    {
        return new Vector3(randomZone.Position.x + Random.Range(-randomZone.Size.x / 2, randomZone.Size.x / 2),
            randomZone.Position.y + Random.Range(-randomZone.Size.y / 2, randomZone.Size.y / 2),
            randomZone.Position.z + Random.Range(-randomZone.Size.z / 2, randomZone.Size.z / 2));
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

    private void OnDrawGizmos()
    {
        if (_spawnZones.Count <= 0) return;
        Gizmos.color = _spawnZoneColor;
        foreach (Zone spawnZone in _spawnZones)
        {
            Gizmos.DrawWireCube(spawnZone.Position, spawnZone.Size);
        }
    }
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
