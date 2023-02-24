using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    #region GET
    public TeamData PlayerTeamData => _playerTeamData;
    #endregion
    
    private TeamData _playerTeamData;

    public void InitialisePlayer(TeamData playerTeamData)
    {
        _playerTeamData = playerTeamData;
    }

}


