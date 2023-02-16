using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelumiTester : MonoBehaviour
{
    [SerializeField] private string testString = "Test";
    [SerializeField] private bool encrypt = false;
    [SerializeField] private TestClass testClass1;
    [SerializeField] private GameEvent gameEvent;
    
    private void OnEnable()
    {
        gameEvent.Register(TriggerEvent);
    }

    private void TriggerEvent(Component arg1, object arg2)
    {
        throw new NotImplementedException();
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
    }
}

[System.Serializable]
public class TestClass
{
    public string[] testString;
}