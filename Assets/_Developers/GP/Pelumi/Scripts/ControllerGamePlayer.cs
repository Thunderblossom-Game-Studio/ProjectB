using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGamePlayer : GamePlayer
{
    private void Start()
    {
        SetUpPakageTracker();
    }

    public void SetUpPakageTracker()
    {
        PackageTracker.Instance.SetCarPakageSystem(gameObject);
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
