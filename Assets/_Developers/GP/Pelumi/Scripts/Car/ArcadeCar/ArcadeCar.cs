using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArcadeCar : MonoBehaviour
{
    public enum Axel
    {
        FRONT,
        REAR
    }

    [Serializable]
    public struct Wheel
    {
        public Transform wheelModel;
        public WheelCollider wheelCollider;
        public TrailRenderer wheelEffectTrail;
        public Axel axel;
    }

    [Header("Acceleration")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float _maxAcceleration = 30.0f;
    [SerializeField] private float _breakAcceleration = 50.0f;
    [SerializeField] private Transform _centreOfMass;
    [Viewable] [SerializeField] private float currentSpeed;
    [Viewable] [SerializeField] private float currentSquareVelocity;

    [Header("Turning")]
    [SerializeField] private float _turnSensitivity = 1.0f;
    [SerializeField] private float _maxSteerAngle = 30.0f;

    [Header("Air")]
    [SerializeField] private float airControl = 10.0f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 1000.0f;

    [Header("References")]
    [SerializeField] private List<Wheel> _wheels;

    [Viewable] [SerializeField] private float _moveInput;
    [Viewable] [SerializeField] private float _steeringInput;
    [Viewable] [SerializeField] private bool _isGrounded = true;

    private Rigidbody _rigidbody;

    private bool isFlipping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _centreOfMass.localPosition;
    }

    private void Start()
    {
        GameMenu.GetInstance()?.SetCarSpeed("0");
    }

    private void Update()
    {
        HandleInput();

        HandleWheelRotation();

        HandleWheelEffect();

        _isGrounded = IsAllWheelGrounded();

        HandleJump();

        currentSquareVelocity = _rigidbody.velocity.sqrMagnitude;
        currentSpeed = _rigidbody.velocity.magnitude;

        float corretSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        GameMenu.GetInstance()?.SetCarSpeed(corretSpeed.ToString("F0"));

        if (Input.GetKeyDown(KeyCode.F))
        {
            FlipCar();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        HandleBrake();
        HandleAirControl();
    }

    public void HandleInput()
    {
        _moveInput = InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().y;
        _steeringInput = InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x;
    }

    public void HandleMovement()
    {
        foreach (Wheel wheel in _wheels)
        {
            wheel.wheelCollider.motorTorque = (currentSpeed < maxSpeed) ? _moveInput * 600 * _maxAcceleration * Time.deltaTime : 0;
        }
    }

    public void HandleSteering()
    {
        foreach (Wheel wheel in _wheels)
        {
            switch (wheel.axel)
            {
                case Axel.FRONT:
                    float steerAngle = _steeringInput * _turnSensitivity * _maxSteerAngle;
                    wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                    break;
                default:  break;
            }
        }
    }

    public void HandleWheelRotation()
    {
        foreach (Wheel wheel in _wheels)
        {
            wheel.wheelCollider.GetWorldPose(out Vector3 _position, out Quaternion _rotation);
            wheel.wheelModel.SetPositionAndRotation(_position, _rotation);
        }
    }

    public void HandleBrake()
    {
        foreach (Wheel wheel in _wheels)
        {
            if (InputManager.Instance.HandleBrakeInput().IsPressed())
            {
                wheel.wheelCollider.brakeTorque = 300 * _breakAcceleration * Time.deltaTime;
            }
            else
            {
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    public void HandleAirControl()
    {
        if (_isGrounded) return;
        _rigidbody.AddForce(transform.forward * _moveInput, ForceMode.Acceleration);
        float newRot = _steeringInput * airControl * Time.deltaTime * _moveInput;
        transform.Rotate(0, newRot, 0, Space.World);
    }

    public void HandleJump()
    {
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void FlipCar()
    {
        StartCoroutine(FlipCarRoutine());
    }

    IEnumerator FlipCarRoutine()
    {
        isFlipping = true;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Wheel wheel in _wheels)
        {
            Gizmos.DrawRay(wheel.wheelModel.position, -Vector3.up * 1);
        }
    }
}
