using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGamePlayer : GamePlayer
{
    [SerializeField] private AICarController carController;
    [SerializeField] private AICombatHandler aICombatHandler;
    [SerializeField] private AIDecisionHandler aIDecisionHandler;
    [SerializeField] private AIPlayerHandler aIPlayerHandler;

    public override void ActivatePlayer()
    {
        carController.enabled = true;
        aICombatHandler.enabled = true;
        aIDecisionHandler.enabled = true;
        aIPlayerHandler.enabled = true;
        base.ActivatePlayer();
    }

    public override void DeactivatePlayer()
    {
        carController.enabled = false;
        aICombatHandler.enabled = false;
        aIDecisionHandler.enabled = false;
        aIPlayerHandler.enabled = false;
        base.DeactivatePlayer();
    }

    public override void HandleDeath(HealthSystem healthSystem)
    {
        base.HandleDeath(healthSystem);
        Respawn(healthSystem);
    }
}
