using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictedMovement : NetworkBehaviour
{
    struct MoveData
    {
        public float Horizontal;
        public float Vertical;

        public MoveData(float horizontal, float vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
    }

    struct ReconcileData
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
        }
    }
}
