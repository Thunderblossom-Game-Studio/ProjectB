using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject camera1;
    [SerializeField] private GameObject camera2;
    private bool cam1Active;
    private bool cam2Active;

    void Start()
    {
        cam1Active = true;
        cam2Active = false;
    }

    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Alpha1)) && (cam1Active == true))
        {
            camera1.SetActive(false);
            camera2.SetActive(true);
            cam1Active = false;
            cam2Active = true;
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha1)) && (cam2Active == true))
        {
            camera2.SetActive(false);
            camera1.SetActive(true);
            cam1Active = true;
            cam2Active = false;
        }
    }
}
