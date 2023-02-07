using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    CarMovement carMovement;
    InputManager inputManager;

    private void Awake()
    {
        carMovement = GetComponent<CarMovement>();    
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        carMovement.Accelerate(inputManager.HandleMoveInput().ReadValue<Vector2>().x, inputManager.HandleMoveInput().ReadValue<Vector2>().y);

        carMovement.Brake(inputManager.HandleBrakeInput().IsPressed());
    }
}
