using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float vehiclePower = 15000f;

    #region Input Variables
    private float horInput;
    private float verInput;
    private bool brakeInput;
    #endregion

    [SerializeField] private VehicleWheel[] vehicleWheels;

    private Rigidbody rigidBody;
    
    public GameObject vehicleCenterOfMass;

    
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();   
    }

    private void Start()
    {
        rigidBody.centerOfMass = vehicleCenterOfMass.transform.localPosition;

        //Lock mouse to center of screen
       // Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleVehicleWheels();
    }

    private void HandleInput()
    {
        //TODO - Input needs to be changed to work with the new input system.
        verInput = Input.GetAxis("Vertical");
        horInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetKey(KeyCode.Space);
        Debug.Log(brakeInput ? "Brake" : "No Brake");
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


