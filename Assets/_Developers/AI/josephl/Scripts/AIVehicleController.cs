using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AIVehicleController : MonoBehaviour
{
    [SerializeField] private float vehiclePower = 15000f;
    [SerializeField] private VehicleWheel[] vehicleWheels;
    
    private Rigidbody rigidBody;

    public GameObject vehicleCenterOfMass;
    
    //Temporary, change for proper values.
    private float horInput;
    private float verInput;
    private bool brakeInput;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigidBody.centerOfMass = vehicleCenterOfMass.transform.localPosition;
    }
    
    private void FixedUpdate()
    {
        HandleVehicleWheels();
    }
    
    private void HandleVehicleWheels()
    {
        foreach (VehicleWheel wheel in vehicleWheels)
        {
            wheel.Steer(horInput);
            wheel.Accelerate(verInput * vehiclePower);
            wheel.Brake(brakeInput);
            wheel.UpdatePosition();

        }
    }



}
