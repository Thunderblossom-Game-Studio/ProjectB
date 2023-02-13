using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkedVehicleController : NetworkBehaviour
{
    [SerializeField] private float vehiclePower = 15000f;

    #region Input Variables
    private float horInput;
    private float verInput;
    private bool brakeInput;
    #endregion

    public NetVehicleWheel[] vehicleWheels;

    private Rigidbody rigidBody;
    
    public GameObject vehicleCenterOfMass;
    [SerializeField] private GameObject cameras;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner)
            return;

        cameras.SetActive(true);
        rigidBody = GetComponent<Rigidbody>();   
        rigidBody.centerOfMass = vehicleCenterOfMass.transform.localPosition;

        //Lock mouse to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        if (!IsOwner)
            return;
        
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;
        
        HandleVehicleWheels();
    }

    private void HandleInput()
    {
        //TODO - Input needs to be changed to work with the new input system.
        verInput = Input.GetAxis("Vertical");
        horInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetKey(KeyCode.Space);
    }

    private void HandleVehicleWheels()
    {     
        foreach (NetVehicleWheel wheel in vehicleWheels)
        {
            wheel.Steer(horInput);
            wheel.Accelerate(verInput * vehiclePower);
            wheel.Brake(brakeInput);

            wheel.LocalUpdatePosition();

            //Debug.Log("Calling ServerRpc");
            //wheel.ServerUpdatePosition(wheel);
        }
    }

}


