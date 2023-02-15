using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedCarController : NetworkBehaviour
{
    [Header("Car Properties")]
    [SerializeField] private float carMotorTorque;
    [SerializeField] private float carBrakeTorque;
    [SerializeField] private float maxCarSpeed;
    [SerializeField] private float maxFuel;
    [SerializeField] private float currentFuel;
    private float steeringAngle = 45f;

    [Header("Car Parts")]
    [SerializeField] private WheelCollider[] CarWheelsCollider; //For assigning wheels in inspector (Wheel Collider, not mesh!)
    [SerializeField] private Transform[] CarWheelsMeshes;
    [SerializeField] private GameObject BrakeLightsOff;
    [SerializeField] private GameObject BrakeLightsOn;

    private VechicleResources vechicleResources;
    private Rigidbody rb;

    [Header("Misc")]
    [SerializeField] private GameObject cameras;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
            return;

        Debug.Log("Setting up car...");

        vechicleResources = GetComponent<VechicleResources>();
        rb = GetComponent<Rigidbody>();

        cameras.SetActive(true);

        try
        {
            bool tmp = NetworkedInputManager.Instance.EnableInput();
            Debug.Log("Enable Input State: " + tmp);
        }
        catch
        {
            Debug.LogError("Input Manager not found");
        }

    }


    public void HandleMotor(float vInput)
    {
        if (!IsOwner)
            return;

        //Car movement forward and backward
        if (rb.velocity.magnitude < maxCarSpeed) //maximum "speed" to accelerate to
        {
            CarWheelsCollider[2].motorTorque = vInput * carMotorTorque;
            CarWheelsCollider[3].motorTorque = vInput * carMotorTorque;

            if (vInput > 0.1f || vInput < -0.1f)
            {
                    vechicleResources.BurnResource("Fuel", vechicleResources._burnRate);
                    //Debug.Log("Fuel: " + VechicleResources.Instance.GetCurrentFuelNormalized());
            }                       
        }

        
        for (int i = 0; i < CarWheelsMeshes.Length; i++)
        {
            UpdateWheelMesh(CarWheelsCollider[i], CarWheelsMeshes[i]);
        }
    }

    public void HandleTurning(float hInput)
    {
        if (!IsOwner)
            return;
        CarWheelsCollider[0].steerAngle = hInput * steeringAngle;
        CarWheelsCollider[1].steerAngle = hInput * steeringAngle;
    }

    public void HandleBraking(bool isBraking)
    {
        if (!IsOwner)
            return;

        if (isBraking)
        {
            foreach (var wheel in CarWheelsCollider)
            {
                wheel.brakeTorque = carBrakeTorque;
                
                BrakeLightsOff.SetActive(false);
                BrakeLightsOn.SetActive(true);
            }
        }
        else
        {
            foreach (var wheel in CarWheelsCollider)
            {
                wheel.brakeTorque = 0;

                BrakeLightsOff.SetActive(true);
                BrakeLightsOn.SetActive(false);
            }
        }
    }

    void UpdateWheelMesh(WheelCollider wheelCol, Transform wheelTrans) //Fixes wheel mesh rotation issue
    {
        if (!IsOwner)
            return;

        Vector3 wheelPos = wheelTrans.position;
        Quaternion wheelRot = wheelTrans.rotation;
        wheelCol.GetWorldPose(out wheelPos, out wheelRot);
        wheelRot = wheelRot * Quaternion.Euler(new Vector3(0, 0, 90));
        wheelTrans.position = wheelPos;
        wheelTrans.rotation = wheelRot;
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        HandleMotor(NetworkedInputManager.Instance.HandleMoveInput().ReadValue<Vector2>().y);
        HandleTurning(NetworkedInputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x);
        HandleBraking(NetworkedInputManager.Instance.HandleBrakeInput().IsPressed());
    }
}