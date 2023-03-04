using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [Viewable][SerializeField] private Transform _target;

    private void Awake()
    {
        Instance = this;
    }

    public void ChangeDistanceToTarget(float newValue)
    {
        CinemachineComponentBase componentBase = _cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer) (componentBase as CinemachineFramingTransposer).m_CameraDistance = newValue;
    }

    public void FollowAndLootAt(Transform target)
    {
        _target = target;
        _cinemachineVirtualCamera.Follow = _target;
        _cinemachineVirtualCamera.LookAt = _target;
    }
}
