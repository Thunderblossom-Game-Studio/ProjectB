using JE.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoDetector : MonoBehaviour
{
    public LayerMask detectingLayers;

    private Volcano volcano;
    private Transform randPos;
    private GameObject targetObj;

    private void Awake() 
    { 
        volcano = GetComponentInParent<Volcano>();
    }

    private void Start()
    {
        targetObj = Instantiate(new GameObject(), transform.position, Quaternion.identity);
    }

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
        if (volcano.targetsPlayer)
        {
            if (!other.gameObject.TryGetComponent(out GamePlayer vehicle)) return;
            volcano.SetTarget(target);
        }
        else
        {
            targetObj.transform.position = RandomPointInCircle(gameObject.transform.localScale.x / 2);
            volcano.SetTarget(targetObj.transform);
        }


        if (target || !volcano.targetsPlayer) HazardIndicator.Instance?.ActivateIndicator(HazardIndicator.IndicatorType.Volcano);
        else HazardIndicator.Instance?.DeActivateIndicator(HazardIndicator.IndicatorType.Volcano);
    }

    Vector3 RandomPointInCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;
        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;
        return new Vector3(x, 0, z) + gameObject.transform.position;
    }
}