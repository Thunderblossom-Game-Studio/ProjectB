using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CameraController : MonoBehaviour
{
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] private GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField] private float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField] private float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    [SerializeField] private float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    [SerializeField] private bool LockCameraPosition = false;
    
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    private const float _threshold = 0.01f;

    void Start()
    {
        
    }

    void Update()
    {
        TPSCameraRotation();
    }

    private void TPSCameraRotation()
    {
        if (InputManager.Instance.HandleLookInput().ReadValue<Vector2>().sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultiplier = (InputManager.Instance.GetCurrentDeviceType() == InputManager.DeviceType.KeyboardAndMouse) ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += InputManager.Instance.HandleLookInput().ReadValue<Vector2>().x * deltaTimeMultiplier;
            _cinemachineTargetPitch += InputManager.Instance.HandleLookInput().ReadValue<Vector2>().y * deltaTimeMultiplier;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
