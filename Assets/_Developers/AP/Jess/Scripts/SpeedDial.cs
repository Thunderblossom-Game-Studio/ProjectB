using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDial : MonoBehaviour
{
    [SerializeField] private float _maximumSpeed = 160;
    [SerializeField] private float _highestAngle = 180;
    [SerializeField] private CarController _carController;

    [SerializeField] private RectTransform _needleCentre;

    public float ReturnNeedleAngle(float currentSpeed, bool antiClockwise)
    {
        float speedPercentage = currentSpeed / _maximumSpeed;
        float needleAngle = _highestAngle * speedPercentage;
        return antiClockwise ? needleAngle : -needleAngle;
    }

    private void Update()
    {
        float currentSpeed = _carController.GetSpeed();
        float needleAngle = ReturnNeedleAngle(currentSpeed, false);
        _needleCentre.rotation = Quaternion.Euler(0f, 0f, needleAngle);
    }
}
