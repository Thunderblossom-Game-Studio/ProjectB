using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private Transform gravityTarget;

    [SerializeField] private float vehiclePower = 15000f;
    [SerializeField] private float vehicleGravity = 9.81f;

    [SerializeField] private bool autoOrient = false;
    [SerializeField] private float autoOrientSpeed = 1f;

    private float horInput;
    private float verInput;
    private float steerAngle;

    public VehicleWheel[] vehicleWheels;

    private Rigidbody rigidBody;


    /// <summary>
    /// The center of mass of the vehicle.
    /// </summary>
    public GameObject vehicleCenterOfMass;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        //rigidBody.centerOfMass = vehicleCenterOfMass.transform.localPosition;
    }

    private void Update()
    {
        HandleInput();
        Vector3 diff = transform.position - gravityTarget.position;
        if (autoOrient) { AutoOrient(-diff); }
    }

    private void FixedUpdate()
    {
        HandleVehicleWheels();
        //HandleGravity();
    }

    private void HandleInput()
    {
        //TODO - Input needs to be changed to work with the new input system.
        verInput = Input.GetAxis("Vertical");
        Debug.Log("Vertical Input: " + verInput);
        horInput = Input.GetAxis("Horizontal");
        Debug.Log("Horizontal Input: " + horInput);
    }

    private void HandleVehicleWheels()
    {
        Debug.Log("Vehicle Wheels: " + vehicleWheels);
        
        foreach (VehicleWheel wheel in vehicleWheels)
        {
            wheel.Steer(horInput);
            wheel.Accelerate(verInput * vehiclePower);
            wheel.UpdatePosition();
        }
    }

    private void HandleGravity()
    {
        Vector3 diff = transform.position - gravityTarget.position;
        rigidBody.AddForce(-diff.normalized * vehicleGravity * (rigidBody.mass));
    }

    private void AutoOrient(Vector3 down)
    {
        Quaternion orientationDirection = Quaternion.FromToRotation(-transform.up, down) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, orientationDirection, autoOrientSpeed * Time.deltaTime);
    }
}


