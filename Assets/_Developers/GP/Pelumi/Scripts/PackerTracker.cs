using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackerTracker : MonoBehaviour
{
    [SerializeField] private EntitySpawner _entitySpawner;

    [Viewable] [SerializeField] private Transform closestPackage;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (_entitySpawner)
        {
            if (_entitySpawner.SpawnedObjects.Count > 0)
            {
                closestPackage = GetClosestPackage(mainCam.transform.gameObject).transform;
                WaypointMarker.Instance?.SetTarget(closestPackage);
            }
        }
    }

    private GameObject GetClosestPackage(GameObject currentPosition)
    {
        SpawnableObject closestPackage = _entitySpawner.SpawnedObjects[0];
        foreach (SpawnableObject spawnableObject in _entitySpawner.SpawnedObjects)
        {
            float distanceBetween = Vector3.Distance(currentPosition.transform.position,
                spawnableObject.transform.position);
            float distanceBetweenOld = Vector3.Distance(currentPosition.transform.position,
                closestPackage.transform.position);

            if (distanceBetween < distanceBetweenOld)
                closestPackage = spawnableObject;
        }
        return closestPackage.gameObject;
    }
}
