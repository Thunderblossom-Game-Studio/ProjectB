using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTracker : Singleton<PackageTracker>
{
    [SerializeField] public List<GameObject> DeliveryPoints;
    [SerializeField] private EntitySpawner _entitySpawner;
    [SerializeField] private PackageSystem PackageSystem;
    [Viewable] [SerializeField] private Transform ObjectToLocate;
    private Camera mainCam;
    bool MaxPackageHeld;
    private void Start()
    {
        mainCam = Camera.main;
        Invoke(nameof(GetCar), 5);
    }

    private void GetCar()
    {
        PackageSystem = GameObject.Find("Main Car(Clone)").GetComponent<PackageSystem>();
    }
    private void Update()
    {
        if (PackageSystem.PackageAmount == PackageSystem.MaxPackages)
        {
            Debug.Log("Working");
            LocateClosestDropPoint();
        }
        else
        {
            Debug.Log("Very Working");
            LocateClosestPackage();
        }

    }

    private void LocateClosestPackage ()
    {
        if (_entitySpawner)
        {
            if (_entitySpawner.SpawnedObjects.Count > 0)
            {
                WaypointMarker.Instance.SetIcon(IconType.Package);
                ObjectToLocate = GetClosestGameObject(mainCam.transform.gameObject, CovertToGameObjectList()).transform;
                WaypointMarker.Instance?.SetTarget(ObjectToLocate);
            }
        }
    }

    private void LocateClosestDropPoint ()
    {
        WaypointMarker.Instance.SetIcon(IconType.Delivery);
        Debug.Log("Aids");
        ObjectToLocate = GetClosestGameObject(mainCam.transform.gameObject, DeliveryPoints).transform;
        WaypointMarker.Instance?.SetTarget(ObjectToLocate);
    }
    private List<GameObject> CovertToGameObjectList()
        
    {
        List<GameObject> GameObjectToSend = new List<GameObject>();
        foreach (var item in _entitySpawner.SpawnedObjects)
        {
            GameObjectToSend.Add(item.gameObject);
        }
        return GameObjectToSend;
    }
    
      
    
    private GameObject GetClosestGameObject(GameObject currentPosition, List<GameObject>gameObjectList)
    {
        GameObject closestPackage = gameObjectList[0];
        foreach (GameObject spawnableObject in gameObjectList)
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
