using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CarType", menuName = "CarType", order = 0)]
public class CarType : ScriptableObject
{
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

    [SerializeField] private DriveType driveType;
    [SerializeField] private BrakeType brakeType;
    [SerializeField] private float carPower;
    [SerializeField] private float brakePower;
    [SerializeField] private int boostPower;
    [SerializeField] private float turningRadius;
    [SerializeField] private float downForceValue;
}
