using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    //Temporary Input
    private float horizontalInput;
    private float verticalInput;
    private bool handbrakeInput;

    internal enum DriveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        FourWheelDrive
    }
    internal enum BrakeType
    {
        FrontWheelBrake,
        RearWheelBrake,
        FourWheelBrake
    }
   
    [SerializeField] private WheelCollider[] wheels = new WheelCollider[4];
    [SerializeField] private GameObject[] wheelMeshes = new GameObject[4];
    private Rigidbody rigidBody;
    private GameObject centerOfMass;
    public float KPH;
    [SerializeField] private int motorTorque = 100;
    [SerializeField] private float brakeTorque = 150;
    [SerializeField] private float steeringAngle = 30;
    [SerializeField] private DriveType driveType = DriveType.FourWheelDrive;
    [SerializeField] private BrakeType brakeType = BrakeType.FourWheelBrake;


    [SerializeField] private float radius = 6;
    public float downForceValue = 50;

    public float[] sidewaysSlip = new float[4];
    public float[] forwardSlip = new float[4];


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        centerOfMass = GameObject.Find("Center_Of_Mass");
        rigidBody.centerOfMass = centerOfMass.transform.localPosition;
    }


    private void Update()
    {
        //Handle Input
        HandleInput();
    }
    
    private void FixedUpdate()
    {
        AddDownForce();

        AnimateWheels();

        HandleAcceleration(verticalInput);
        
        HandleSteering(horizontalInput);

        HandleBraking(handbrakeInput);

        GetFriction();
    }

    

    private void HandleAcceleration(float vInput)
    {
        if (driveType == DriveType.FourWheelDrive)
        {
            foreach (WheelCollider wheel in wheels)
            {
                wheel.motorTorque = vInput * (motorTorque / 4);
            }
        }
        else if (driveType == DriveType.FrontWheelDrive)
        {
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = vInput * (motorTorque / 2);
            }
        }
        else if (driveType == DriveType.RearWheelDrive)
        {
            for (int i = 0; i < wheels.Length - 2; i++)
            {
                wheels[i].motorTorque = vInput * (motorTorque / 2);
            }
        }

        KPH = rigidBody.velocity.magnitude * 3.6f;
    }

    private void HandleBraking(bool brakeInput)
    {
        if (brakeInput)
        {
            if(brakeType == BrakeType.RearWheelBrake)
            {
                wheels[2].brakeTorque = brakeTorque;
                wheels[3].brakeTorque = brakeTorque;
            }
            else if (brakeType == BrakeType.FrontWheelBrake)
            {
                wheels[0].brakeTorque = brakeTorque;
                wheels[1].brakeTorque = brakeTorque;
            }
            else if (brakeType == BrakeType.FourWheelBrake)
            {
                foreach (WheelCollider wheel in wheels)
                {
                    wheel.brakeTorque = brakeTorque;
                }
            }
        }
        else
        {
            foreach (WheelCollider wheel in wheels)
            {
                wheel.brakeTorque = 0;
            }
        }
    }
    
    private void HandleSteering(float hInput)
    {    
        //Ackerman steering formula
        if(hInput > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.5f / (radius + (1.5f / 2))) * hInput;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.5f / (radius - (1.5f / 2))) * hInput;
        }
        else if(hInput < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.5f / (radius - (1.5f / 2))) * hInput;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.5f / (radius + (1.5f / 2))) * hInput;
        }
        else
        {
            wheels[0].steerAngle = 0;
            wheels[1].steerAngle = 0;
        }
    }

    private void AnimateWheels()
    {
        Vector3 wheePosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].GetWorldPose(out wheePosition, out wheelRotation);
            wheelMeshes[i].transform.position = wheePosition;
            wheelMeshes[i].transform.rotation = wheelRotation;
        }
    }
    
    private void HandleInput()
    {
        //Replace with new input system
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        handbrakeInput = Input.GetKey(KeyCode.Space);
    }

    private void AddDownForce()
    {
        rigidBody.AddForce(-transform.up * downForceValue * rigidBody.velocity.magnitude);
    }

    private void GetFriction()
    {
        for (int i = 0; i < wheels.Length; i++)
        {
            WheelHit wheelHit;
            wheels[i].GetGroundHit(out wheelHit);

            sidewaysSlip[i] = wheelHit.sidewaysSlip;
            forwardSlip[i] = wheelHit.forwardSlip;

        }
    }
}

