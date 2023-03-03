using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PropellerMovement : MonoBehaviour
{
    public enum Direction
    {
        Right, Left
    }

    private float speed = 5.0f;
    [SerializeField] private Direction direction;

    void Update()
    {
        transform.Rotate(0.0f, 0.0f, direction == Direction.Right ? speed : -speed);
    }

    public void ChangeSpeed(float setValue)
    {
        speed = setValue;
    }
}
