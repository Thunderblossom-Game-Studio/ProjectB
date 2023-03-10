using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{

    public UnityEvent onPlayerTrigger;

    bool acted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (acted) return;

        if (other.CompareTag("Player") && other.TryGetComponent<CarController>(out CarController c))
        {
            onPlayerTrigger.Invoke();
        }

        acted = true;
    }
}
