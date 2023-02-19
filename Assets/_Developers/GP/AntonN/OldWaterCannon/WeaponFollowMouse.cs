using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFollowMouse : MonoBehaviour
{
    [SerializeField] private Transform weaponCam;
    [SerializeField] private Transform weaponOrientation;
    [SerializeField] private float aimSensitivity = 200f;
    [SerializeField] private float rotationSpeed = 5f;
    private float xRot;
    private float desiredXRot;

    private void Start()
    {
        weaponCam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Cursor crosshair
        float mouseX = Input.GetAxis("Mouse X") * aimSensitivity * Time.fixedDeltaTime * 1;
        float mouseY = Input.GetAxis("Mouse Y") * aimSensitivity * Time.fixedDeltaTime * 1;
        Vector3 rot = weaponCam.transform.localRotation.eulerAngles;
        desiredXRot = rot.y + mouseX;
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90f);
        weaponCam.transform.localRotation = Quaternion.Euler(xRot, desiredXRot, 0);
        weaponOrientation.transform.localRotation = Quaternion.Euler(0, desiredXRot, 0);

        //Rotation
        Quaternion targetRotation = Quaternion.Euler(0, weaponCam.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
