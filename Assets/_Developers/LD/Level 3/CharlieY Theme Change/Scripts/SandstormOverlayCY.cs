using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandstormOverlayCY : MonoBehaviour
{
    public GameObject SandStormOverlay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent<ControllerGamePlayer>(out ControllerGamePlayer gamePlayer))
        {
            SandStormOverlay.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root.TryGetComponent<ControllerGamePlayer>(out ControllerGamePlayer gamePlayer))
        {
            SandStormOverlay.SetActive(false);
        }
    }

}