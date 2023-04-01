using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeCarController : MonoBehaviour
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
    [SerializeField] private float maxBoostSpeed;
    [SerializeField] private float maxForwardSpeed;
    [SerializeField] private float maxReverseSpeed;
    [Viewable] [SerializeField] private float currentMaxSpeed;

    [Header("Acceleration")]
    [SerializeField] private float carPower;
    [SerializeField] private float startAccelerationRate;
    [SerializeField] private float startAccelerationSpeedThreshold;

    [Header("Acceleration / Decceleration")]
    [SerializeField] private float accelerationToDecelerationSpeed = 5;

    [Header("Decceleration")]
    [SerializeField] private float decelerationForce;
    [SerializeField] private float driftingDecelerationRate;

    [Header("Brake")]
    [SerializeField] private float brakePower;
    [SerializeField] private float breakDecelerationRate;

    [Header("Steering")]
    [SerializeField] private float _steeringSensitivity = 1.0f;
    [SerializeField] private float maxSteeringAngle = 35;

    [Header("Boost")]
    [SerializeField] private float boostForce = 200f;
    [SerializeField] private float boostDuration = 2f;
    [SerializeField] private float boostReductionSpeed;
    [SerializeField] private float boostRegenerationSpeed;

    [SerializeField] private float resetBoostSpeed;

    [SerializeField] private ParticleSystem boostParticle;
    [Viewable] [SerializeField] private bool isBoosting;
    [Viewable] [SerializeField] private float currentBoostDuration;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 1000.0f;
    [Viewable] [SerializeField] private bool _isGrounded = true;

    [Header("Air Control")]
    [SerializeField] private float airHorizontalForce = 10.0f;
    [SerializeField] private float downForceValue;

    [Header("Debug")]
    [Viewable] [SerializeField] private float currentSpeed;
    [Viewable] [SerializeField] private float topSpeedDrag;
    [Viewable] [SerializeField] private float idleDrag = 50f;

    [Viewable] [SerializeField] private float horizontalInput;
    [Viewable] [SerializeField] private float verticalInput;
    [Viewable] [SerializeField] private bool handbrakeInput;
    [Viewable] [SerializeField] private bool boostInput;

    [Viewable] [SerializeField] private bool isFlipping;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMass.transform.localPosition;

        foreach (Wheel wheel in _wheels)
        {
            wheel.wheelCollider.ConfigureVehicleSubsteps(5, 12, 15);
        }
    }

    private void Update()
    {
        _isGrounded = IsAnyWheelGrounded();

        HandleWheelEffect();

        ClampCarSpeed();
    }

    private void FixedUpdate()
    {
        HandleAirControl();

        HandleEngine();

        HandleBrakingAndDeceleration();

        HandleWheelRotation();

        HandleCarDrift();

        HandleDrag();

        HandleBoost();

        HandleDownForce();
    }

    public void SetHorizontalAndVerticalInput(float _horizontalInput, float _verticalInput)
    {
        horizontalInput = _horizontalInput;
        verticalInput = _verticalInput;
    }

    public void SetBreakInput(bool isBreaked)
    {
        handbrakeInput = isBreaked;
    }

    public void SetBoost(bool _boostInput)
    {
        boostInput = _boostInput;
    }

    private void HandleEngine()
    {
        float motor = carPower * verticalInput;
        float steering = maxSteeringAngle * _steeringSensitivity * horizontalInput;
        currentSpeed = rigidBody.velocity.magnitude;

        HandleSteering(steering);

        if (motor != 0f)
        {
            HandleAcceleration(motor);
        }

        foreach (Wheel wheel in _wheels)
        {
            if (wheel.wheelCollider.rpm > 400 && verticalInput == 0)   wheel.wheelCollider.motorTorque = 0;
        }
    }

    private void HandleAcceleration(float motor)
    {
        if (_isGrounded)
        {
            if (verticalInput > 0 && currentSpeed < startAccelerationSpeedThreshold)
            {
                rigidBody.velocity += transform.forward * startAccelerationRate * Time.deltaTime;
            }
            else  if (verticalInput < 0)
            {
                if (Vector3.Dot(transform.forward, rigidBody.velocity) > 0)
                {
                    rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, Vector3.zero, accelerationToDecelerationSpeed * Time.deltaTime);
                }
            }

        }

        if (driveType == DriveType.FourWheelDrive)
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.wheelCollider.brakeTorque = 0;
                wheel.wheelCollider.motorTorque = currentSpeed < maxForwardSpeed ?  motor :  0;
            }
        }
        else if (driveType == DriveType.RearWheelDrive)
        {
            foreach (Wheel wheel in _wheels)
            {
                switch (wheel.axel)
                {
                    case Axel.REAR:
                        wheel.wheelCollider.brakeTorque = 0;
                        wheel.wheelCollider.motorTorque = currentSpeed < maxForwardSpeed ? motor : 0;
                        break;
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
                    case Axel.FRONT:
                        wheel.wheelCollider.brakeTorque = 0;
                        wheel.wheelCollider.motorTorque = currentSpeed < maxForwardSpeed ?  motor :  0;
                        break;
                    default: break;
                }
            }
        }
    }

    private void HandleBrakingAndDeceleration()
    {
        if (handbrakeInput || verticalInput == 0)
        {
            if (rigidBody.velocity.magnitude > 0)
            {
                rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, Vector3.zero, (horizontalInput == 0 && handbrakeInput) ?  breakDecelerationRate : driftingDecelerationRate * Time.deltaTime);
            }

            switch (brakeType)
            {
                case BrakeType.FrontWheelBrake:

                    foreach (Wheel wheel in _wheels)
                    {
                        switch (wheel.axel)
                        {
                            case Axel.FRONT:
                                wheel.wheelCollider.brakeTorque = handbrakeInput ?  brakePower : decelerationForce;
                                break;
                            default: break;
                        }
                    }

                    break;
                case BrakeType.RearWheelBrake:

                    foreach (Wheel wheel in _wheels)
                    {
                        switch (wheel.axel)
                        {
                            case Axel.REAR:
                                wheel.wheelCollider.brakeTorque = handbrakeInput ? brakePower : decelerationForce; break;
                            default: break;
                        }
                    }

                    break;
                case BrakeType.FourWheelBrake:

                    foreach (Wheel wheel in _wheels)
                    {
                        wheel.wheelCollider.brakeTorque = handbrakeInput ? brakePower : decelerationForce;
                    }

                    break;
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

    private void HandleSteering(float steering)
    {
        if (steering != 0)
        {
            foreach (Wheel wheel in _wheels)
            {
                switch (wheel.axel)
                {
                    case Axel.FRONT:
                        wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steering, 0.6f);
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

    private void HandleWheelRotation()
    {
        foreach (Wheel wheel in _wheels)
        {
            wheel.wheelCollider.GetWorldPose(out Vector3 _position, out Quaternion _rotation);
            wheel.wheelModel.SetPositionAndRotation(_position, _rotation);
        }
    }

    private void HandleDownForce()
    {
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

    public void ClampCarSpeed()
    {
        CalculateMaxSpeed();
        rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, currentMaxSpeed);
    }

    public void CalculateMaxSpeed()
    {
        if (isBoosting) currentMaxSpeed = maxBoostSpeed;
        else
        {
            if (currentSpeed > maxForwardSpeed)
            {
                float speed = currentMaxSpeed;
                currentMaxSpeed = Mathf.Lerp(speed, maxForwardSpeed, resetBoostSpeed * Time.deltaTime);
            }
            else 
            {
                float speed = currentMaxSpeed;
                currentMaxSpeed = Mathf.Lerp(speed, verticalInput > 0 ? maxForwardSpeed : maxReverseSpeed, resetBoostSpeed * Time.deltaTime);
            }
        }
    }

    void HandleDrag()
    {
        if (currentSpeed >= maxForwardSpeed)
        {
            rigidBody.drag = topSpeedDrag;
        }
        else if (carPower == 0)
        {
            rigidBody.drag = idleDrag;
        }
    }

    void HandleCarDrift()
    {
        WheelHit hit;
        float radius;
        float velocity = rigidBody.velocity.magnitude;
        WheelFrictionCurve wheelFowardFriction;
        WheelFrictionCurve wheelSideFriction;
        foreach (Wheel wheel in _wheels)
        {
            switch (wheel.axel)
            {
                case Axel.REAR:
                    if (wheel.wheelCollider.GetGroundHit(out hit))
                    {
                        radius = 4 + (-Mathf.Abs(hit.sidewaysSlip) * 2) + rigidBody.velocity.magnitude / 10;

                        wheelFowardFriction = wheel.wheelCollider.forwardFriction;
                        wheelFowardFriction.stiffness = (handbrakeInput) ? Mathf.SmoothDamp(wheel.wheelCollider.forwardFriction.stiffness, .5f, ref velocity, Time.deltaTime * 2) : 2;
                        wheel.wheelCollider.forwardFriction = wheelFowardFriction;

                        wheelSideFriction = wheel.wheelCollider.sidewaysFriction;
                        wheelSideFriction.stiffness = (handbrakeInput) ? Mathf.SmoothDamp(wheel.wheelCollider.sidewaysFriction.stiffness, .5f, ref velocity, Time.deltaTime * 2) : 2;
                        wheel.wheelCollider.sidewaysFriction = wheelSideFriction;
                    }
                    break;
                default: break;
            }
        }
    }

    public float GetSpeed() => currentSpeed;

    public void HandleBoost()
    {
        if (boostInput)
        {
            if (currentBoostDuration > 0f)
            {
                isBoosting = true;
                if (_isGrounded) rigidBody.AddForce(transform.forward * boostForce, ForceMode.Acceleration);
                currentBoostDuration -= boostReductionSpeed * Time.deltaTime;
            }
            else
            {
                isBoosting = false;
            }
        }
        else
        {
            isBoosting = false;
            if (currentBoostDuration < boostDuration)
            {
                currentBoostDuration += boostRegenerationSpeed * Time.deltaTime;
            }
        }
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void FlipCar()
    {
        if (!isFlipping)
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
            wheel.wheelEffectTrail.emitting = InputManager.Instance.HandleBrakeInput().IsPressed() && wheel.axel == Axel.REAR && wheel.wheelCollider.isGrounded && rigidBody.velocity.magnitude >= 10f;
        }

        if (isBoosting)
        {
            boostParticle.Emit(1);
        }
    }

    public bool IsAnyWheelGrounded()
    {
        foreach (Wheel wheel in _wheels)
        {
            if (wheel.wheelCollider.isGrounded) return true;
        }
        return false;
    }

    public bool IsAllWheelGrounded()
    {
        foreach (Wheel wheel in _wheels)
        {
            if (!wheel.wheelCollider.isGrounded) return false;
        }
        return true;
    }

    public void ToggleWheelActive(bool state)
    {
        foreach (Wheel wheel in _wheels)
        {
            wheel.wheelCollider.enabled = state;
        }
    }
}
