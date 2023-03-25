using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTriggerCheck : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabs;

    internal bool active = false;

    private void OnTriggerStay(Collider other)
    {
        if (!_prefabs.Contains(other.gameObject))
            active = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_prefabs.Contains(other.gameObject))
            active = false;
    }
}
