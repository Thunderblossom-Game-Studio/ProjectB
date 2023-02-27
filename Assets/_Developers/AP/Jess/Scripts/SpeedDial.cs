using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDial : MonoBehaviour
{
    [SerializeField] private float _maximumSpeed = 160;
    [SerializeField] private float _highestAngle = 180;
    [SerializeField] private RectTransform _needleCentre;

    [SerializeField] private GameEvent carSpeedEvent;

    private void OnEnable()
    {
        carSpeedEvent.Register(CarSpeedEvent);
    }

    private void CarSpeedEvent(Component arg1, object arg2)
    {
        float currentSpeed = (float)arg2;
        float needleAngle = ReturnNeedleAngle(currentSpeed, false);
        _needleCentre.rotation = Quaternion.Euler(0f, 0f, needleAngle);
    }

    public float ReturnNeedleAngle(float currentSpeed, bool antiClockwise)
    {
        float speedPercentage = currentSpeed / _maximumSpeed;
        float needleAngle = _highestAngle * speedPercentage;
        return antiClockwise ? needleAngle : -needleAngle;
    }

    private void OnDisable()
    {
        carSpeedEvent.Unregister(CarSpeedEvent);
    }
}
