using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITestCar : MonoBehaviour
{

    [SerializeField] private float minAccSpeed = 0f;
    [SerializeField] private float maxAccSpeed = 30f;

    [SerializeField] private float moveMulit = 100f;

    [SerializeField] private float rotSpeed = .3f;

    private Rigidbody rb;

    private Vector3 frameMovement;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody Found");

            return;
        }

    }

    private void FixedUpdate()
    {
        rb.AddForce(frameMovement, ForceMode.Force);
    }

    public void CustomAccelerate(int dir, float speed)
    {
        speed = Mathf.Clamp(speed, minAccSpeed, maxAccSpeed);

        frameMovement = transform.forward * speed * moveMulit * dir;
    }

    public void Accelerate(int dir)
    {
        frameMovement = transform.forward * maxAccSpeed * moveMulit * dir;
    }

    public void Turn(int dir)
    {
        if (rb.velocity.magnitude > 0.5f)
        {
            transform.Rotate(Vector3.up * rotSpeed * dir * Time.deltaTime);
        }
    }
}
