using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject CinemachineCameraTarget;

    [Range(0.01f, 1.0f)]
    [SerializeField] private float sensitivity = 70.0f;
    [SerializeField] private float TopClamp = 70.0f;
    [SerializeField] private float BottomClamp = -30.0f;   
    [SerializeField] private float CameraAngleOverride = 0.0f;
    
    private float cinemachineTargetYaw;
    private float cinemachineTargetPitch;
    private const float threshold = 0.01f;

    void Update()
    {
        TPSCameraRotation();
    }

    private void TPSCameraRotation()
    {
        if (InputManager.Instance.HandleLookInput().ReadValue<Vector2>().sqrMagnitude >= threshold)
        {
            float deltaTimeMultiplier = (InputManager.Instance.GetCurrentDeviceType() == InputManager.DeviceType.KeyboardAndMouse) ? 1.0f : Time.deltaTime;

            cinemachineTargetYaw += InputManager.Instance.HandleLookInput().ReadValue<Vector2>().x * deltaTimeMultiplier * sensitivity;
            cinemachineTargetPitch += InputManager.Instance.HandleLookInput().ReadValue<Vector2>().y * deltaTimeMultiplier * sensitivity;
        }

        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride, cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
