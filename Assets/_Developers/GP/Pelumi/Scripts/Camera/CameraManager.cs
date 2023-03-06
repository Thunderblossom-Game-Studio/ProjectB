using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private bool debugZoom;
    [Viewable][SerializeField] private Transform _target;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (debugZoom) DebugZoom();
    }

    public void ChangeDistanceToTarget(float newValue)
    {
        CinemachineComponentBase componentBase = _cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is Cinemachine3rdPersonFollow cinemachine3RdPersonFollow)
        {          
            cinemachine3RdPersonFollow.CameraDistance = Mathf.Lerp(cinemachine3RdPersonFollow.CameraDistance, newValue, 5 * Time.deltaTime);
        } 
    }

    public void FollowAndLootAt(Transform target)
    {
        _target = target;
        _cinemachineVirtualCamera.Follow = _target;
        _cinemachineVirtualCamera.LookAt = _target;
    }

    public void DebugZoom()
    {
        if (Mouse.current.press.wasPressedThisFrame) Debug.Log("Mouse Pressed");
        if (Mouse.current.press.wasReleasedThisFrame) Debug.Log("Mouse Released");
        ChangeDistanceToTarget(Mouse.current.rightButton.IsPressed() ? 1.5f : 5);
    }
}
