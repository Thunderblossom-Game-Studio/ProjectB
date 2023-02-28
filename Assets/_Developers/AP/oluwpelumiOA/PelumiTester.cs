using Pelumi.Juicer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class PelumiTester : MonoBehaviour
{
    [SerializeField] private string testString = "Test";
    [SerializeField] private bool encrypt = false;
    [SerializeField] private TestClass testClass1;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private GameObject debugObject;
    [SerializeField] private GameObject spawnPosition;
    [SerializeField] private GameObject landPosition;
    [SerializeField] private float speed = 10;

    [Header("Train")]
    [SerializeField] private int trainRouteDebugSize = 20;
    [SerializeField] private float trainMoveSpeed = 10;
    [SerializeField] private float trainRotateSpeed = 10;
    [SerializeField] private Transform trainHead;
    [SerializeField] private List<Transform> trainParts;
    [SerializeField] private int partDistance = 1;
    [SerializeField] private List<Vector3> trainRoute;

    [Header("Debug")]
    [SerializeField] private Vector3 nextTrainRoute;

    private void Start()
    {
        trainHead.position = trainRoute[0];
        nextTrainRoute = trainRoute[1];
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLoadManager.Save("Test", testClass1, encrypt);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            testClass1 = SaveLoadManager.Load<TestClass>("Test", encrypt);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Juicer.DoMoveSmooth(debugObject.transform, transform.position, landPosition.transform.position , speed,  45f));
        }

        Train();
    }

    public void DebugTouch()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycast, 999f, detectLayer))
        {
            debugObject.transform.position = raycast.point;
            Debug.DrawLine(ray.origin, raycast.point, Color.yellow);
        }
    }

    public void Train()
    {
        MoveToWayPoint();
        TrainPartFollow();
        CheckDistanceToWayPoint();
        RotateToFaceTarget(trainHead, nextTrainRoute, trainRotateSpeed);
    }

    public void MoveToWayPoint()
    {
        trainHead.position = Vector3.MoveTowards(trainHead.position, nextTrainRoute, trainMoveSpeed * Time.deltaTime);
    }

    public void TrainPartFollow()
    {
        for (int i = 1; i < trainParts.Count; i++)
        {
            Transform currentPart = trainParts[i];
            Transform previousPart = trainParts[i - 1];
            Vector3 targetPosition = previousPart.position + previousPart.forward * partDistance;
            currentPart.position = Vector3.Slerp(currentPart.position, targetPosition, trainMoveSpeed * Time.deltaTime);
            RotateToFaceTarget(currentPart, previousPart.position, trainRotateSpeed);
        }
    }

    public void RotateToFaceTarget(Transform theObject, Vector3 targetPos, float rotationSpeed)
    {
        Quaternion rotation = Quaternion.LookRotation(theObject.position - targetPos);
        theObject.rotation = Quaternion.Slerp(theObject.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    public void CheckDistanceToWayPoint()
    {
        if (Vector3.Distance(trainHead.position, nextTrainRoute) <= 0.2f)
        {
            nextTrainRoute = trainRoute[(nextTrainRoute == trainRoute[^1]) ? 0 : trainRoute.IndexOf(nextTrainRoute) + 1];
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = trainRouteDebugSize;

        for (int i = 0; i < trainRoute.Count; i++)
        {
            Gizmos.DrawWireSphere(trainRoute[i], .2f);

            #if UNITY_EDITOR
            Handles.Label(trainRoute[i], "Route" + (i + 1), gUIStyle);
            #endif
        } 

        Gizmos.color = Color.green;

        Vector3 startPos1 = trainRoute[0];

        for (int i = 0; i < trainRoute.Count; i++)
        {
            Gizmos.DrawLine(startPos1, trainRoute[i]);
            startPos1 = trainRoute[i];
        }

        Gizmos.DrawLine(trainRoute[0], trainRoute[^1]);
    }
}

[System.Serializable]
public class TestClass
{
    public string[] testString;
}