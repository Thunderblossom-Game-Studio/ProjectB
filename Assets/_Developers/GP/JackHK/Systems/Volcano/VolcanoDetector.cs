using JE.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoDetector : MonoBehaviour
{
    public LayerMask detectingLayers;

    private Volcano volcano;
    public GameObject targetObj;
    public GameObject randomSpot;


    private void Awake()
    {
        volcano = GetComponentInParent<Volcano>();
    }

    private void Start()
    {
        targetObj = Instantiate(new GameObject(), transform.position, Quaternion.identity);
    }

    private bool CheckForPlayer(Collider potentialPlayer)
    {
        if (detectingLayers.ContainsLayer(potentialPlayer.gameObject.layer) && potentialPlayer.gameObject.CompareTag("Player")) return true;
        else return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckForPlayer(other)) return;
        ChangeDetectionState(other, other.transform);
        AudioManager.PlaySoundEffect("Erruption");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!CheckForPlayer(other)) return;
        ChangeDetectionState(other, null);
    }

    private void ChangeDetectionState(Collider other, Transform target)
    {
        if (volcano.targetsPlayer)
        {
            volcano.SetTarget(target);
        }
        else
        {
            targetObj.transform.position = volcano.RandomVector(randomSpot.transform.position);
            volcano.SetTarget(targetObj.transform);
        }

        if (target || !volcano.targetsPlayer) HazardIndicator.Instance?.ActivateIndicator(HazardIndicator.IndicatorType.Volcano);
        else HazardIndicator.Instance?.DeActivateIndicator(HazardIndicator.IndicatorType.Volcano);
    }
}