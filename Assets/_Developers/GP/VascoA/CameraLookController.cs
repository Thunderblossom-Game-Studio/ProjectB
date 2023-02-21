using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookController : MonoBehaviour
{
    float mouseX;
    float mouseY;
    public float rotationPower;


    private void Update()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationPower;
        mouseY += Input.GetAxis("Mouse Y") * rotationPower;

        //Clamp the up/down rotation
        mouseY = Mathf.Clamp(mouseY, -60, 60);

        //Rotate cameraFollow
        transform.localEulerAngles = new Vector3(-mouseY, mouseX, 0);

        //Lock mouse
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}
