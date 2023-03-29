using JE.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoDetector : MonoBehaviour
{
    public LayerMask detectingLayers;

    private Volcano volcano;
    private Transform randPos;

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
        if (!other.gameObject.TryGetComponent(out GamePlayer vehicle)) return;

        if (volcano.targetsPlayer) volcano.SetTarget(target);
        else
        {
            volcano.SetTarget(randPos);
        }


        if (target) HazardIndicator.Instance?.ActivateIndicator(HazardIndicator.IndicatorType.Volcano);
        else HazardIndicator.Instance?.DeActivateIndicator(HazardIndicator.IndicatorType.Volcano);
    }

    Vector3 RandomPointInCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;
        return new Vector3(x, 0, z);
    }
}