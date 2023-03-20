using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashDetector : MonoBehaviour
{

    [SerializeField] private AICarController owner;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EditorOnly"))
        {
            //owner.crashed = true;
        }
    }
}