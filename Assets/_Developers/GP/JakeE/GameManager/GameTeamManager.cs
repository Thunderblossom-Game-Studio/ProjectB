using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTeamManager : MonoBehaviour
{
    [SerializeField] private TeamData _blueTeamData;
    [SerializeField] private TeamData _redTeamData;
    [SerializeField] private List<GamePlayer> _gamePlayers;
    
    public void InitialiseTeams()
    {
        bool toggleTeam = false;
        foreach (GamePlayer gamePlayer in _gamePlayers)
        {
            if (toggleTeam == false)
            {
                toggleTeam = true;
                _blueTeamData.AddPlayer(gamePlayer);
                gamePlayer.InitialisePlayer(_blueTeamData);
            }
            else
            {
                toggleTeam = false;
                _redTeamData.AddPlayer(gamePlayer);
                gamePlayer.InitialisePlayer(_redTeamData);
            }
        }
    }

    public TeamData GetWinningTeam()
    {
        return _redTeamData.TeamPoints > _blueTeamData.TeamPoints
            ? _blueTeamData : _redTeamData;
    }
}

[Serializable]
public class TeamData
{
    #region GET

    public int TeamPoints => _teamPoints;
    public List<Vector3> SpawnPoints => _spawnPoints;
    public List<GamePlayer> TeamPlayers => _teamPlayers;
    public string TeamName => _teamName;

    #endregion

    public Action<int> OnScoreUpdate;
    [SerializeField] private string _teamName;
    [SerializeField] private int _teamPoints;
    [SerializeField] private List<Vector3> _spawnPoints;
    [SerializeField] private List<GamePlayer> _teamPlayers = new List<GamePlayer>();
    
    public void AddPlayer(GamePlayer gamePlayer) => _teamPlayers.Add(gamePlayer);
    public void RemovePlayer(GamePlayer gamePlayer) => _teamPlayers.Remove(gamePlayer);
    public bool ContainsPlayer(GamePlayer gamePlayer) => _teamPlayers.Contains(gamePlayer);
    public void AddSpawn(Vector3 spawnPosition) => _spawnPoints.Add(spawnPosition);
    
    public void AddScore(int score)
    {
        _teamPoints += score;
        OnScoreUpdate?.Invoke(score);
    }
    public void RemoveScore(int score)
    {
        _teamPoints -= score;
        OnScoreUpdate?.Invoke(score);
    }
}
