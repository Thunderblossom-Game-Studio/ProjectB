using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetVehicleWheel : NetworkBehaviour
{
    [SyncVar][SerializeField] private bool isWheelPowered = false;
    [SyncVar][SerializeField] private float maxWheelAngle = 90f;
    [SyncVar][SerializeField] private float wheelOffset = 0f;
    [SyncVar][SerializeField] private float brakePower = 1000.0f;

    private float wheelTurnAngle;
    [NonSerialized] public WheelCollider wheelCollider;
    [NonSerialized] public Transform wheelMesh;

    public override void OnStartClient()
    {
        base.OnStartClient();

        //if (!IsOwner)
        //    return;

        wheelCollider = GetComponentInChildren<WheelCollider>();
        wheelMesh = transform.Find("WheelMesh");
    }

    public void Steer(float steerInput)
    {
        if (!IsOwner)
            return;

        wheelTurnAngle = steerInput * maxWheelAngle + wheelOffset;
        wheelCollider.steerAngle = wheelTurnAngle;
    }

    public void Accelerate(float powerInput)
    {
        if (!IsOwner)
            return;

        if (isWheelPowered) wheelCollider.motorTorque = powerInput;
        else wheelCollider.brakeTorque = 0;
    }

    public void Brake(bool brakeInput)
    {
        if (!IsOwner)
            return;

        if (brakeInput) wheelCollider.brakeTorque = brakePower;
        else wheelCollider.brakeTorque = 0;

    }

    [ServerRpc]
    public void ServerUpdatePosition(NetVehicleWheel wheel)
    {
        Debug.Log("Calling Client Rpc");
        OberserverUpdatePosition(wheel);
    }

    [ObserversRpc(IncludeOwner = true)]
    public void OberserverUpdatePosition(NetVehicleWheel wheel)
    {
        Debug.Log("Client Rpc Called!");

        wheel.wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheel.wheelMesh.transform.SetPositionAndRotation(position, rotation);
    }

    public void LocalUpdatePosition()
    {
        wheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        wheelMesh.transform.SetPositionAndRotation(position, rotation);

        ServerUpdatePosition(this);
    }
}
