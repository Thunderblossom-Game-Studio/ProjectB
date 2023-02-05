using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashDetector : MonoBehaviour
{

    [SerializeField] private AICarController owner;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ground"))
        {
            owner.crashed = true;
        }
    }
}
