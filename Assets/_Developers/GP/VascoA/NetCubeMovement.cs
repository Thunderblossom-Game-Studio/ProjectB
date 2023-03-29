
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetCubeMovement : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10f;
    public float force = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * force);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * force);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * force);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * force);
        }

    }
}
