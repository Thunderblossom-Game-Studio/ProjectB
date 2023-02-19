using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    CarControllerOLD carMovement;
    InputManager inputManager;


    float inputH;

    private void Awake()
    {
        carMovement = GetComponent<CarControllerOLD>();
        Debug.Assert(carMovement != null, "CarController not found");
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        carMovement.HandleMotor(inputManager.HandleMoveInput().ReadValue<Vector2>().y);
        carMovement.HandleTurning(inputManager.HandleMoveInput().ReadValue<Vector2>().x);
        carMovement.HandleBraking(inputManager.HandleBrakeInput().IsPressed());
    }
    
}
