using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassOn : MonoBehaviour
{
    public Transform connect;
    public GameObject child;

    public void Pass()
    {
        child.GetComponent<TrafficBrain>().goal = connect;
    }
}
