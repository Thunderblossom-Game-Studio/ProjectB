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
            InitialisePlayer(GameTeamManager.Instance._blueTeamData);
            GameTeamManager.Instance._blueTeamData.AddPlayer(this);
        }
    }

    public void OnHealthChanged(float normalisedValue)
    {
        GameMenu.GetInstance()?.SetHealthView(normalisedValue);
    }

    public void HandleDeath(HealthSystem healthSystem)
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


