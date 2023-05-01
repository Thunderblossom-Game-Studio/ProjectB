using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGamePlayer : GamePlayer
{
    [SerializeField] private ArcadeCarController carController;
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private GameObject deathCam;

    private void Start()
    {
        SetUpPakageTracker();
    }

    private void Update()
    {
        carController.SetBreakInput(InputManager.Instance.HandleBrakeInput().IsPressed());

        carController.SetBoost(InputManager.Instance.HandleBoostInput().IsPressed());

        if(InputManager.Instance.HandleJumpInput().WasPressedThisFrame())
        {
            carController.Jump();
        }

        GameMenu.GetInstance()?.SetCarSpeed(carController.GetSpeed().ToString("F0") + " Km/h");
        GameMenu.GetInstance()?.SetBoostBar(carController.NormalisedBoostAmount());
    }

    private void FixedUpdate()
    {
        Vector2 input = InputManager.Instance.HandleMoveInput().ReadValue<Vector2>();
        carController.SetHorizontalAndVerticalInput(input.x, input.y);
    }

    public override void ActivatePlayer()
    {
        carController.enabled = true;
        weaponController.enabled = true;
        base.ActivatePlayer();
    }

    public override void DeactivatePlayer()
    {
        carController.enabled = false;
        weaponController.enabled = false;
        base.DeactivatePlayer();
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
        base.HandleDeath(healthSystem);
        deathCam.SetActive(true);
    }

    public override void Respawn(HealthSystem healthSystem)
    {
        base.Respawn(healthSystem);
        deathCam.SetActive(false);
    }
}
