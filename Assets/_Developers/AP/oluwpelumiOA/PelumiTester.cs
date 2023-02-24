using Pelumi.Juicer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

[System.Serializable]
public class TestClass
{
    public string[] testString;
}