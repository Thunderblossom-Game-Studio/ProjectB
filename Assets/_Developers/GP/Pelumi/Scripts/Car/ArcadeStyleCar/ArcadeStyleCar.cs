using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeStyleCar : MonoBehaviour
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
    [SerializeField] private bool userControlled = true;

    [Header("Wheel Settings")]
    [SerializeField] private List<Wheel> _wheels;
    [SerializeField] private Transform _centreOfMass;

    [Header("Acceleration")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float motorPower;
    [Viewable] [SerializeField] private float gasInput;

    [Header("Braking")]
    [SerializeField] private float brakePower;
    [SerializeField] private bool isBreaking;
    [Viewable] [SerializeField] private float brakeInput;

    [Header("Steering")]
    [SerializeField] private AnimationCurve steeringCurve;
    [Viewable] [SerializeField] private float steeringInput;
    [Viewable] [SerializeField] private float currentSteeringAngle;
    [Viewable] [SerializeField] private float maxSteeringAngle = 90f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 1000.0f;
    [Viewable] [SerializeField] private bool _isGrounded = true;

    [Header("Effects")]
    [SerializeField] private GameObject smokePrefab;

    private float slipAngle;
    private float speed;
    private Rigidbody carRB;
    private bool isFlipping;

    private void Awake()
    {
        carRB = GetComponent<Rigidbody>();
       // carRB.centerOfMass = _centreOfMass.localPosition;
    }

    private void Start()
    {
        GameMenu.GetInstance()?.SetCarSpeed("0");
    }

    public float SetSteeringInput(float input)
    {
        return steeringInput = input;
    }

    public float SetGasInput(float input)
    {
        return gasInput = input;
    }

    public float SetBrakeInput(float input)
    {
        return brakeInput = input;
    }

    void Update()
    {
        speed = carRB.velocity.magnitude;
        float corretSpeed = Mathf.Clamp(speed, 0, maxSpeed);
        GameMenu.GetInstance()?.SetCarSpeed(corretSpeed.ToString("F0"));

        CheckInput();
        HandleBreaking();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
        CheckParticles();
        HandleWheelRotation();
        HandleJump();

        if (Input.GetKeyDown(KeyCode.F))
        {
            FlipCar();
        }
    }


    private void FixedUpdate()
    {

    }

    void CheckInput()
    {
        if (userControlled)
        {
            gasInput = InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().y;
            steeringInput = InputManager.Instance.HandleMoveInput().ReadValue<Vector2>().x;
        }

        slipAngle = Vector3.Angle(transform.forward, carRB.velocity - transform.forward);
        isBreaking = InputManager.Instance.HandleBrakeInput().IsPressed();
    }

    void ApplyBrake()
    {
        foreach (Wheel wheel in _wheels)
        {
            switch (wheel.axel)
            {
                case Axel.FRONT: wheel.wheelCollider.brakeTorque = brakeInput * brakePower * 0.7f; break;
                case Axel.REAR: wheel.wheelCollider.brakeTorque = brakeInput * brakePower * 0.3f; break;
                default: break;
            }
        }
    }

    void HandleBreaking()
    {
        if (isBreaking)
        {
            brakeInput = 1f;
        }
        else
        {
            float movingDirection = Vector3.Dot(transform.forward, carRB.velocity);
            if (movingDirection < -0.5f && gasInput > 0)
            {
                brakeInput = Mathf.Abs(gasInput);
            }
            else if (movingDirection > 0.5f && gasInput < 0)
            {
                brakeInput = Mathf.Abs(gasInput);
            }
            else
            {
                brakeInput = 0;
            }
        }
    }

    void ApplyMotor()
    {
        foreach (Wheel wheel in _wheels)
        {
            switch (wheel.axel)
            {
                case Axel.REAR: wheel.wheelCollider.motorTorque = motorPower * gasInput; break;
                default: break;
            }
        }
    }

    void ApplySteering()
    {
        currentSteeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        if (slipAngle < 120f)
        {
            currentSteeringAngle += Vector3.SignedAngle(transform.forward, carRB.velocity + transform.forward, Vector3.up);
        }
        currentSteeringAngle = Mathf.Clamp(currentSteeringAngle, -maxSteeringAngle, maxSteeringAngle);

        foreach (Wheel wheel in _wheels)
        {
            switch (wheel.axel)
            {
                case Axel.FRONT: wheel.wheelCollider.steerAngle = currentSteeringAngle;   break;
                default: break;
            }
        }
    }

    void CheckParticles()
    {
        foreach (Wheel wheel in _wheels)
        {
            wheel.wheelCollider.GetGroundHit(out WheelHit  wheelHits);
            float slipAllowance = 0.5f;
            if ((Mathf.Abs(wheelHits.sidewaysSlip) + Mathf.Abs(wheelHits.forwardSlip) > slipAllowance))
            {
                wheel.particleSystem.Play();
            }
            else
            {
                wheel.particleSystem.Stop();
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
    public void HandleJump()
    {
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            carRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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