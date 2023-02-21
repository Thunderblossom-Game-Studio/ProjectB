using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class PredictedMovement : NetworkBehaviour
{
    private struct MoveData : IReplicateData
    {
        public float Horizontal;
        public float Vertical;
        public MoveData(float horizontal, float vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;

            _tick = 0;
        }

        private uint _tick;
        public void Dispose() { }
        public uint GetTick() => _tick;
        public void SetTick(uint value) => _tick = value;
    }

    private struct ReconcileData : IReconcileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
        {
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;

            _tick = 0;
        }

        private uint _tick;
        public void Dispose() { }
        public uint GetTick() => _tick;
        public void SetTick(uint value) => _tick = value;
    }

    public float MoveRate = 30f;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (base.IsServer || base.IsClient)
            base.TimeManager.OnTick += TimeManager_OnTick;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        if (base.TimeManager != null)
            base.TimeManager.OnTick -= TimeManager_OnTick;
    }

    private void TimeManager_OnTick()
    {
        if (base.IsOwner)
        {
            Reconcile(default, false);
            BuildActions(out MoveData md);
            Move(md, false);
        }
        if (base.IsServer)
        {
            Move(default, true);
            ReconcileData rd = new ReconcileData(transform.position, transform.rotation, _rigidbody.velocity, _rigidbody.angularVelocity);
            Reconcile(rd, true);
        }
    }

    private void BuildActions(out MoveData data)
    {
        //Set to default.
        data = default;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (horizontal == 0f && vertical == 0f)
            return;

        data = new MoveData(horizontal, vertical);
    }


    [Replicate]
    private void Move(MoveData data, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
    {
        Vector3 force = new Vector3(data.Horizontal, 0f, data.Vertical) * MoveRate;
        _rigidbody.AddForce(force);
    }

    [Reconcile]
    private void Reconcile(ReconcileData data, bool asServer, Channel channel = Channel.Unreliable)
    {
        transform.SetPositionAndRotation(data.Position, data.Rotation);
        _rigidbody.velocity = data.Velocity;
        _rigidbody.angularVelocity = data.AngularVelocity;
    }

}