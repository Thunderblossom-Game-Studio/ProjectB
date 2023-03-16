using JE.General;
using RVP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoDetector : MonoBehaviour
{
    public LayerMask detectingLayers;

    private Volcano volcano;

    private void Awake() { volcano = GetComponentInParent<Volcano>(); }

    private void OnTriggerEnter(Collider other)
    {
        ChangeDetectionState(other, other.transform);
        AudioManager.PlaySoundEffect("Erruption");

    }

    private void OnTriggerExit(Collider other)
    {
        ChangeDetectionState(other, null);
    }

    private void ChangeDetectionState(Collider other, Transform target)
    {
        if (!other.gameObject.TryGetComponent(out VehicleParent vehicle)) return;

        volcano.SetTarget(target);

        if(target) HazardIndicator.Instance?.ActivateIndicator(HazardIndicator.IndicatorType.Volcano);
        else HazardIndicator.Instance?.DeActivateIndicator(HazardIndicator.IndicatorType.Volcano);
    }
}