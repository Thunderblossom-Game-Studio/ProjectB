using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTriggerCheck : MonoBehaviour
{
    internal bool active = false;

    private void OnTriggerStay(Collider other)
    {
        active = true;
    }

    private void OnTriggerExit(Collider other)
    {
        active = false;
    }
}
