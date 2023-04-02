using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGamePlayer : GamePlayer
{
    [SerializeField] private ArcadeCarController carController;

    private void Start()
    {
        SetUpPakageTracker();
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.F))
       {
            carController.FlipCar();
       }

        if(Input.GetKeyDown(KeyCode.J))
        {
            carController.Jump();
        }

        carController.SetBreakInput(InputManager.Instance.HandleBrakeInput().IsPressed());

        carController.SetBoost(Input.GetKey(KeyCode.LeftShift));

        GameMenu.GetInstance()?.SetCarSpeed(carController.GetSpeed().ToString("F0"));
    }

    private void FixedUpdate()
    {
        Vector2 input = InputManager.Instance.HandleMoveInput().ReadValue<Vector2>();
        carController.SetHorizontalAndVerticalInput(input.x, input.y);
    }

    public void SetUpPakageTracker()
    {
        PackageTracker.Instance?.SetCarPakageSystem(gameObject);
    }

    public void OnHealthChanged(float normalisedValue)
    {
        GameMenu.GetInstance()?.SetHealthView(normalisedValue);
    }

    public override void HandleDeath(HealthSystem healthSystem)
    {
        if (!gameObject.TryGetComponent(out GamePlayer gamePlayer))
            return;

        transform.position = gamePlayer.PlayerTeamData.GetRandomSpawnPoint();
        transform.rotation = Quaternion.identity;

        healthSystem.RestoreHealth(healthSystem.MaximumHealth);

        if (!gameObject.TryGetComponent(out Rigidbody currentBody))
            return;

        currentBody.velocity = Vector3.zero;
    }
}
