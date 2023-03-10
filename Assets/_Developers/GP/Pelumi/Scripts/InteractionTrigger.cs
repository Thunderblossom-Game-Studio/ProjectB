using JE.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private UnityEvent eventToTrigger;

    [Tooltip("Number of time the trigger will work [0  = Unlimied]")]
    [SerializeField] private int maxTrigger;

    [Viewable] [SerializeField] private int currentTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (!detectLayer.ContainsLayer(other.gameObject.layer) || IsUsedUp()) return;

        eventToTrigger?.Invoke();

        if (maxTrigger > 0) ++currentTrigger;
    }

    public bool IsUsedUp() => maxTrigger > 0 && currentTrigger >= maxTrigger;
}
