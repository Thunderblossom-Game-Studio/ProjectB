using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCameraFollow : MonoBehaviour
{
    private CarController carController;
    public GameObject Player;
    public GameObject Child;
    public float cameraMoveSpeed;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Child = Player.transform.Find("Camera_Constraint").gameObject;
        carController = Player.GetComponent<CarController>();
    }

    private void Start()
    {
        transform.position = Child.transform.position;
    }

    private void FixedUpdate()
    {
        Follow();

        cameraMoveSpeed = (carController.GetSpeed() >= 50) ? 10 : carController.GetSpeed() / 5;
    }

    private void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, Child.transform.position, Time.deltaTime * cameraMoveSpeed);
        transform.LookAt(Player.transform);
    }
}
