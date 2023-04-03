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

    [SerializeField] private bool addOnAwake = true;

    private TeamData _playerTeamData;

    public void InitialisePlayer(TeamData playerTeamData)
    {
        _playerTeamData = playerTeamData;
    }

    private void Awake()
    {
        if (addOnAwake)
        {
            GameTeamManager.Instance._gamePlayers.Add(this);
            GameTeamManager.Instance.InitialiseTeams();
        }
    }

    public virtual void HandleDeath(HealthSystem healthSystem)
    {
        if (!gameObject.TryGetComponent(out GamePlayer gamePlayer))
            return;

        transform.position = gamePlayer.PlayerTeamData.GetRandomSpawnPoint();
        transform.rotation = Quaternion.identity;
        
        healthSystem.RestoreHealth(healthSystem.MaximumHealth);
        
        if (!gameObject.TryGetComponent(out Rigidbody currentBody)) 
            return;
        
        currentBody.velocity = Vector3.zero;
        
        if (!gameObject.TryGetComponent(out PackageSystem packageSystem))
            return;
        
        packageSystem.DropPackages();
        //packageSystem.ClearPackageData();
    }
}


