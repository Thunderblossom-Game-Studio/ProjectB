using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
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
}


