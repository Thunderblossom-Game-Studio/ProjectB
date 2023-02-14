using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleWheel : MonoBehaviour
{
    [SerializeField] private bool isWheelPowered = false;
    [SerializeField] private float maxWheelAngle = 90f;
    [SerializeField] private float wheelOffset = 0f;
    [SerializeField] private float brakePower = 1000.0f;
    
    private float wheelTurnAngle;
    private WheelCollider wheelCollider;
    private Transform wheelMesh;

    private void Awake()
    {
        wheelCollider = GetComponentInChildren<WheelCollider>();
        wheelMesh = transform.Find("WheelMesh");
    }

    public void Steer(float steerInput)
    {
        wheelTurnAngle = steerInput * maxWheelAngle + wheelOffset;
        wheelCollider.steerAngle = wheelTurnAngle;
    }

    public void Accelerate(float powerInput)
    {
        if (isWheelPowered) wheelCollider.motorTorque = powerInput;
        else wheelCollider.brakeTorque = 0;
    }

    public void Brake(bool brakeInput)
    {
        if (brakeInput) wheelCollider.brakeTorque = brakePower;
        else wheelCollider.brakeTorque = 0;

    }
    public void UpdatePosition()
    {  
        wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheelMesh.transform.SetPositionAndRotation(position, rotation);
    }





}
