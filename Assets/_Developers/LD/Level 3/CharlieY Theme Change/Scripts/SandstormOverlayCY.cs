using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandstormOverlayCY : MonoBehaviour
{
    public GameObject SandStormOverlay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name== "Main Car(Clone)")
        {
            SandStormOverlay.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Main Car(Clone)")
        {
            SandStormOverlay.SetActive(false);
        }
    }

}
