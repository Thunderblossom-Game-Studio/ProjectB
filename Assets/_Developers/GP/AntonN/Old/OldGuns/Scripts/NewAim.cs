using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAim : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private bool isInstant = false;

    Camera cam1 = null;

    void Start()
    {
        cam1 = Camera.main;
    }

    private void Update()
    {

        Ray ray = cam1.ScreenPointToRay(Input.mousePosition);
        Vector3 mouseDirection = ray.direction;

        Quaternion targetRotation = Quaternion.LookRotation(mouseDirection);

        if (isInstant)
        {
            transform.rotation = targetRotation;
        }
        else
        {
            Quaternion currentRotation = transform.rotation;
            float angularDifference = Quaternion.Angle(currentRotation, targetRotation);

            if (angularDifference > 0)
            {
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, (rotationSpeed * 180 * Time.deltaTime) / angularDifference);
            }
            else
            {
                transform.rotation = targetRotation;
            }

        }
    }

}
