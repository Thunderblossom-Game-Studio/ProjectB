using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Packets
public struct MoveData : IReplicateData
{
    public float horizontalInput;
    public float verticalInput;
    public bool handbrakeInput;
    public bool boostInput;

    MoveData(float h, float v, bool brake, bool boost)
    {
        horizontalInput = h;
        verticalInput = v;
        handbrakeInput = brake;
        boostInput = boost;

        _tick = 0;
    }

    private uint _tick;
    public void Dispose() { }
    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;
}

struct ReconcileData : IReconcileData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public WheelData[] wheelData;

    ReconcileData(Vector3 pos, Quaternion rot, Vector3 vel, WheelData[] wd)
    {
        position = pos;
        rotation = rot;
        velocity = vel;
        wheelData = wd;

        _tick = 0;
    }

    private uint _tick;
    public void Dispose() { }
    public uint GetTick() => _tick;
    public void SetTick(uint value) => _tick = value;
}

public struct WheelData
{
    public Vector3 position;
    public Quaternion rotation;
    public float rpm;
    public float torque;

    WheelData(Vector3 pos, Quaternion rot, float rpm, float torque)
    {
        position = pos;
        rotation = rot;
        this.rpm = rpm;
        this.torque = torque;
    }
}

#endregion


public class PredictedCarController : NetworkBehaviour
{
    #region Variables
    private GameObject wheelMeshes, wheelColliders;
    private WheelCollider[] wheels = new WheelCollider[4];
    private GameObject[] wheelMesh = new GameObject[4];
    private Rigidbody rigidBody;
    private GameObject centerOfMass;
    
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

    [Header("Car Properties")]
    [SerializeField] private DriveType driveType = DriveType.FourWheelDrive;
    [SerializeField] private BrakeType brakeType = BrakeType.FourWheelBrake;
    [SerializeField] private float topSpeed;
    [SerializeField] private float carPower;
    [SerializeField] private float brakePower;
    [SerializeField] private int boostPower;
    [SerializeField] private float turningRadius;
    [Viewable] [SerializeField] private float downForceValue;
    [Viewable] [SerializeField] private float currentSpeed;

    private float topSpeedDrag, idleDrag = 50f;


    private float horizontalInput;
    private float verticalInput;
    private bool handbrakeInput;
    private bool boostInput;
    #endregion

    private void Awake()
    {
        GetGameObjects();
        
        rigidBody.centerOfMass = centerOfMass.transform.localPosition;
    }

    #region ConnectionHandling
    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (base.IsServer || base.IsClient)
            base.TimeManager.OnTick += TimeManager_OnTick;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        if (base.TimeManager != null)
            base.TimeManager.OnTick -= TimeManager_OnTick;
    }
    #endregion

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        AnimateWheels();

        HandleAcceleration(verticalInput);

        HandleSteering(horizontalInput);

        HandleBraking(handbrakeInput);

        AdjustDrag();

        AddDownForce();
    }

    private void TimeManager_OnTick()
    {
        if (IsOwner)
        {
            
        }
        if (IsServer)
        {
            
        }
    }

    private void CheckInput(out MoveData data)
    {
        data = default;
        data.horizontalInput = horizontalInput;
        data.verticalInput = verticalInput;
        data.handbrakeInput = handbrakeInput;
        data.boostInput = boostInput;
    }

    [Replicate]
    private void Move(MoveData data, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
    {

    }

    [Reconcile]
    private void Reconcile(ReconcileData data, bool asServer, Channel channel = Channel.Unreliable)
    {

    }
    




    #region Original
    void AdjustDrag()
    {
        if (currentSpeed >= topSpeed)
        {
            rigidBody.drag = topSpeedDrag;
        }
        else if (carPower == 0)
        {
            rigidBody.drag = idleDrag;
        }
    }

    private void AddDownForce()
    {
        downForceValue = currentSpeed / 2;

        rigidBody.AddForce(-transform.up * downForceValue * rigidBody.velocity.magnitude);
    }

    private void GetGameObjects()
    {
        wheelMeshes = GameObject.Find("Wheel_Meshes");
        for (int i = 0; i < wheelMeshes.transform.childCount; i++)
        {
            wheelMesh[i] = wheelMeshes.transform.GetChild(i).gameObject;
        }

        wheelColliders = GameObject.Find("Wheel_Colliders");
        for (int i = 0; i < wheelColliders.transform.childCount; i++)
        {
            wheels[i] = wheelColliders.transform.GetChild(i).GetComponent<WheelCollider>();
        }

        rigidBody = GetComponent<Rigidbody>();

        centerOfMass = GameObject.Find("Center_Of_Mass");

    }

    private void HandleAcceleration(float vInput)
    {
        if (driveType == DriveType.FourWheelDrive)
        {
            foreach (WheelCollider wheel in wheels)
            {
                wheel.motorTorque = vInput * (carPower / 4);
            }
        }
        else if (driveType == DriveType.RearWheelDrive)
        {
            for (int i = 2; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = vInput * (carPower / 2);
            }
        }
        else if (driveType == DriveType.FrontWheelDrive)
        {
            for (int i = 0; i < wheels.Length - 2; i++)
            {
                wheels[i].motorTorque = vInput * (carPower / 2);
            }
        }

        currentSpeed = rigidBody.velocity.magnitude * 3.6f;



        //Boost
        if (boostInput)
        {
            rigidBody.AddForce(transform.forward * boostPower);
        }
    }

    private void HandleBraking(bool brakeInput)
    {
        if (brakeInput)
        {
            if (brakeType == BrakeType.RearWheelBrake)
            {
                wheels[2].brakeTorque = brakePower;
                wheels[3].brakeTorque = brakePower;
            }
            else if (brakeType == BrakeType.FrontWheelBrake)
            {
                wheels[0].brakeTorque = brakePower;
                wheels[1].brakeTorque = brakePower;
            }
            else if (brakeType == BrakeType.FourWheelBrake)
            {
                foreach (WheelCollider wheel in wheels)
                {
                    wheel.brakeTorque = brakePower;
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
        if (hInput > 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.5f / (turningRadius + (1.5f / 2))) * hInput;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.5f / (turningRadius - (1.5f / 2))) * hInput;
        }
        else if (hInput < 0)
        {
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.5f / (turningRadius - (1.5f / 2))) * hInput;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.5f / (turningRadius + (1.5f / 2))) * hInput;
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
            wheelMesh[i].transform.position = wheePosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }

    private void HandleInput()
    {
        //Replace with new input system
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        handbrakeInput = Input.GetKey(KeyCode.Space);
        boostInput = Input.GetKey(KeyCode.LeftShift);
    }

    public float GetSpeed() => currentSpeed;
    #endregion

    private void OnGUI()
    {
        //Change gui color
        GUI.color = Color.black;
        //Change gui size
        GUI.skin.label.fontSize = 15;
        //Show speed on screen
        GUI.Label(new Rect(10, 10, 150, 100), "Speed: " + currentSpeed.ToString("0") + " KPH");
    }
}
