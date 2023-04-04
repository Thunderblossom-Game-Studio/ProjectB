using System;
using System.Collections;
using System.Collections.Generic;
using JE.DamageSystem;
using UnityEngine;

public class GamePlayer : MonoBehaviour, IKillable
{
    #region GET
    public TeamData PlayerTeamData => _playerTeamData;
    #endregion

    private TeamData _playerTeamData;

    private bool activated = false;

    public bool IsActivated => activated;

    [SerializeField] private GameObject bodyVisual;
    [SerializeField] private float respawnDelay = 2;

    public void InitialisePlayer(TeamData playerTeamData)
    {
        _playerTeamData = playerTeamData;
    }

    public virtual void ActivatePlayer()
    {
        activated = true;
    }

    public virtual void DeactivatePlayer()
    {
        activated = true;
    }

    public virtual void HandleDeath(HealthSystem healthSystem)
    {
        if (!gameObject.TryGetComponent(out Rigidbody currentBody)) return;
        currentBody.velocity = Vector3.zero;
        bodyVisual.SetActive(false);
        if (!gameObject.TryGetComponent(out PackageSystem packageSystem)) return;
        packageSystem.DropPackages();
        DeactivatePlayer();
        StartCoroutine(RespawnDelay(healthSystem));
    }

    IEnumerator RespawnDelay(HealthSystem healthSystem)
    {
        yield return new WaitForSeconds(respawnDelay);
        Respawn(healthSystem);
    }

    public virtual void Respawn(HealthSystem healthSystem)
    {
        transform.position = _playerTeamData.GetRandomSpawnPoint();
        transform.rotation = Quaternion.identity;     
        healthSystem.RestoreHealth(healthSystem.MaximumHealth);
        bodyVisual.SetActive(true);
        ActivatePlayer();
    }
}


