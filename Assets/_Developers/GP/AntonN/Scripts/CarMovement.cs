using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private WheelCollider[] CarWheelsCollider; //For assigning wheels in inspector (Wheel Collider, not mesh!)
    [SerializeField] private GameObject BrakeLightsOff;
    [SerializeField] private GameObject BrakeLightsOn;
    [SerializeField] private float BrakingForce;
    [SerializeField] private Transform[] CarWheelsMeshes;
    [SerializeField] private float torque;
    private float angle = 45f;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Accelerate(float hInput, float vInput)
    {
        for (int i = 0; i < CarWheelsCollider.Length; i++)
        {
            //Car movement forward and backward using W/S and Up/Down arrowkeys
            if (rb.velocity.magnitude < 3) //maximum "speed" to accelerate to
            {
                CarWheelsCollider[i].motorTorque = vInput * torque;
                //Car turning
                if (i == 0 || i == 1) //First two wheels should be front wheels
                {
                    CarWheelsCollider[i].steerAngle = hInput * angle; //Turn using A/D and Left/Right arrowkeys
                }
                print(rb.velocity.magnitude); //speed of rigidbody
            }
            else
            {
                CarWheelsCollider[i].motorTorque = vInput;
                if (i == 0 || i == 1) //First two wheels should be front wheels
                {
                    CarWheelsCollider[i].steerAngle = hInput * angle; //Turn using A/D and Left/Right arrowkeys
                }
            }
        }

        for (int i = 0; i < CarWheelsMeshes.Length; i++)
        {
            UpdateWheelMesh(CarWheelsCollider[i], CarWheelsMeshes[i]);
        }
    }

    public void Brake(bool isBraking)
    {
        if (isBraking)
        {
            foreach (var i in CarWheelsCollider)
            {
                i.brakeTorque = BrakingForce;
                BrakeLightsOff.SetActive(false);
                BrakeLightsOn.SetActive(true);
            }
        }
        else
        {
            foreach (var i in CarWheelsCollider)
            {
                i.brakeTorque = 0; //resets brakeTorque so car can move again
                BrakeLightsOff.SetActive(true);
                BrakeLightsOn.SetActive(false);
            }
        }
    }

    void UpdateWheelMesh(WheelCollider wheelCol, Transform wheelTrans) //Fixes wheel mesh rotation issue
    {
        Vector3 wheelPos = wheelTrans.position;
        Quaternion wheelRot = wheelTrans.rotation;
        wheelCol.GetWorldPose(out wheelPos, out wheelRot);
        wheelRot = wheelRot * Quaternion.Euler(new Vector3(0, 0, 90));
        wheelTrans.position = wheelPos;
        wheelTrans.rotation = wheelRot;
    }
}