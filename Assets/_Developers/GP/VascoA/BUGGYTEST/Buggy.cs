using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Buggy : MonoBehaviour
{
    public Transform gravityTarget;

    public float power = 15000f;
    public float torque = 500f;
    public float gravity = 9.81f;

    public bool autoOrient = false;
    public float autoOrientSpeed = 1f;

    private float horInput;
    private float verInput;
    private float steerAngle;

     public Wheel[] wheels;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update() 
    {
        ProcessInput();
        Vector3 diff = transform.position - gravityTarget.position;
        if(autoOrient) { AutoOrient(-diff); }
    }

    void FixedUpdate()
    {
        ProcessForces();
        ProcessGravity();
    }

    void ProcessInput()
    {
        verInput = Input.GetAxis("Vertical");
        horInput = Input.GetAxis("Horizontal");
    }

    void ProcessForces()
    {
        //Vector3 force = new Vector3(0f, 0f, verInput * power);
        //rb.AddRelativeForce(force);

        //Vector3 rforce = new Vector3(0f, horInput* torque, 0f);
        //rb.AddRelativeTorque(rforce);

        foreach(Wheel w in wheels)
        {
            w.Steer(horInput);
            w.Accelerate(verInput * power);
            w.UpdatePosition();
        }
    }

    void ProcessGravity()
    {
        Vector3 diff = transform.position - gravityTarget.position;
        rb.AddForce(-diff.normalized * gravity * (rb.mass));
    }

    void AutoOrient(Vector3 down)
    {
        Quaternion orientationDirection = Quaternion.FromToRotation(-transform.up, down) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, orientationDirection, autoOrientSpeed * Time.deltaTime);
    }
}