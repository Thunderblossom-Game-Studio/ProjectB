using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedCar : MonoBehaviour
{
    public enum Axel
    {
        FRONT,
        REAR
    }

    [Serializable]
    public struct Wheel
    {
        public Axel axel;
        public Transform wheelModel;
        public WheelCollider wheelCollider;
        public TrailRenderer wheelEffectTrail;
        public ParticleSystem particleSystem;
    }

    [Header("Wheel Settings")]
    [SerializeField] private List<Wheel> _wheels;

    private Rigidbody rigidBody;
    [SerializeField] private GameObject centerOfMass;
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

    [Header("Acceleration / Break")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float carPower;
    [SerializeField] private float brakePower;

    [Header("Boost")]
    [SerializeField] private float boostSpeed = 200f;
    [SerializeField] private float boostDuration = 2f;

    [Header("Turning")]
    [SerializeField] private float turningRadius;
    [SerializeField] private float _turnSensitivity = 1.0f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 1000.0f;
    [Viewable] [SerializeField] private bool _isGrounded = true;

    [Header("Air Control")]
    [SerializeField] private float airHorizontalForce = 10.0f;

    [Header("Debug")]
    [Viewable] [SerializeField] private float downForceValue;
    [Viewable] [SerializeField] private float currentSpeed;
    [Viewable] [SerializeField] private float topSpeedDrag;
    [Viewable] [SerializeField] private float idleDrag = 50f;

    [Viewable] [SerializeField] private float horizontalInput;
    [Viewable] [SerializeField] private float verticalInput;
    [Viewable] [SerializeField] private bool handbrakeInput;
    [Viewable] [SerializeField] private bool boostInput;
    [Viewable] [SerializeField] private bool jumpInput;
    [Viewable] [SerializeField] private bool flipInput;

    [Viewable] [SerializeField] private bool isFlipping;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void Update()
    {
        _isGrounded = IsAllWheelGrounded();

        HandleInput();

        FlipCar();

        HandleWheelEffect();
    }

    private void FixedUpdate()
    {
        AnimateWheels();

        HandleAirControl();

        HandleAcceleration(verticalInput);

        HandleSteering(horizontalInput);

        HandleBraking(handbrakeInput);

        AdjustDrag();

        HandleJump();

        Boost();

        AddDownForce();
    }

    private void HandleInput()
    {
        verticalInput = InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().y;
        horizontalInput = InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x;
        handbrakeInput = InputManager.Instance.HandleBrakeInput().IsPressed();
        boostInput = Input.GetKey(KeyCode.LeftShift);
        flipInput = Input.GetKeyDown(KeyCode.F);
        jumpInput = Input.GetKeyDown(KeyCode.Space);
    }

    private void HandleAcceleration(float vInput)
    {
        if (driveType == DriveType.FourWheelDrive)
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.wheelCollider.motorTorque = vInput * (carPower / 4);
            }
        }
        else if (driveType == DriveType.RearWheelDrive)
        {
            foreach (Wheel wheel in _wheels)
            {
                switch (wheel.axel)
                {
                    case Axel.REAR: wheel.wheelCollider.motorTorque = vInput * (carPower / 2); break;
                    default: break;
                }    
            }
        }
        else if (driveType == DriveType.FrontWheelDrive)
        {
            foreach (Wheel wheel in _wheels)
            {
                switch (wheel.axel)
                {
                    case Axel.FRONT: wheel.wheelCollider.motorTorque = vInput * (carPower / 2); break;
                    default: break;
                }
            }
        }

        currentSpeed = rigidBody.velocity.magnitude * 3.6f;

        foreach (Wheel wheel in _wheels)
        {
            if (wheel.wheelCollider.rpm > 400 && vInput == 0)
                wheel.wheelCollider.motorTorque = 0;
        }
    }

    private void HandleBraking(bool brakeInput)
    {
        if (brakeInput)
        {
            if (brakeType == BrakeType.RearWheelBrake)
            {
                foreach (Wheel wheel in _wheels)
                {
                    switch (wheel.axel)
                    {
                        case Axel.REAR: wheel.wheelCollider.brakeTorque = brakePower; break;
                        default: break;
                    }
                }
            }
            else if (brakeType == BrakeType.FrontWheelBrake)
            {
                foreach (Wheel wheel in _wheels)
                {
                    switch (wheel.axel)
                    {
                        case Axel.FRONT: wheel.wheelCollider.brakeTorque = brakePower; break;
                        default: break;
                    }
                }
            }
            else if (brakeType == BrakeType.FourWheelBrake)
            {
                foreach (Wheel wheel in _wheels)
                {
                    wheel.wheelCollider.brakeTorque = brakePower;
                }
            }
        }
        else
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    private void HandleSteering(float hInput)
    {
        if (hInput != 0)
        {
            foreach (Wheel wheel in _wheels)
            {
                switch (wheel.axel)
                {
                    case Axel.FRONT:
                        float steerAngle = hInput * _turnSensitivity * turningRadius;
                        wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                        break;
                    default: break;
                }
            }
        }
        else
        {
            _wheels[0].wheelCollider.steerAngle = 0;
            _wheels[1].wheelCollider.steerAngle = 0;
        }
    }

    private void AnimateWheels()
    {
        foreach (Wheel wheel in _wheels)
        {
            wheel.wheelCollider.GetWorldPose(out Vector3 _position, out Quaternion _rotation);
            wheel.wheelModel.SetPositionAndRotation(_position, _rotation);
        }
    }

    private void AddDownForce()
    {
        downForceValue = currentSpeed / 2;
        rigidBody.AddForce(-transform.up * downForceValue * rigidBody.velocity.magnitude);
    }

    public void HandleAirControl()
    {
        if (!_isGrounded && airHorizontalForce > 0)
        {
            rigidBody.velocity += transform.right * horizontalInput * airHorizontalForce * Time.deltaTime;

            Vector3 horizontalVelocity = rigidBody.velocity;
            horizontalVelocity.y = 0f;
            if (horizontalVelocity.magnitude > 0.1f)
            {
                Quaternion airRotation = Quaternion.LookRotation(horizontalVelocity);
                transform.rotation = Quaternion.Lerp(transform.rotation, airRotation, Time.deltaTime * 2f);
            }
        }
    }

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

    public float GetSpeed() => currentSpeed;

    public void Boost()
    {
        if (boostInput)
        {
            if (boostDuration > 0f)
            {
                rigidBody.AddForce(transform.forward * boostSpeed, ForceMode.Acceleration);
                boostDuration -= Time.deltaTime;
            }
        }
    }

    public void HandleJump()
    {
        if (_isGrounded && jumpInput)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void FlipCar()
    {
        if(flipInput && !isFlipping)
        {
            isFlipping = true;
            StartCoroutine(FlipCarRoutine());
        }
    }

    IEnumerator FlipCarRoutine()
    {
        ToggleWheelActive(false);
        yield return new WaitForSeconds(.1f);
        transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(.1f);
        ToggleWheelActive(true);
        isFlipping = false;
    }

    public void HandleWheelEffect()
    {
        foreach (Wheel wheel in _wheels)
        {
            wheel.wheelEffectTrail.emitting = InputManager.Instance.HandleBrakeInput().IsPressed() && wheel.axel == Axel.REAR && wheel.wheelCollider.isGrounded;
        }
    }

    public bool IsAllWheelGrounded()
    {
        foreach (Wheel wheel in _wheels)
        {
            if (wheel.wheelCollider.isGrounded) return true;
        }
        return false;
    }

    public void ToggleWheelActive(bool state)
    {
        foreach (Wheel wheel in _wheels)
        {
            wheel.wheelCollider.enabled = state;
        }
    }
}
