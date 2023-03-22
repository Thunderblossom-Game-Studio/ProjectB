using System;
using System.Collections.Generic;
using JE.General;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameTeamManager : Singleton<GameTeamManager>
{
    [Header("Game Events")]
    [SerializeField] private GameEvent _onBlueTeamScore;
    [SerializeField] private GameEvent _onRedTeamScore;
    
    [Header("Team Data")]
    [SerializeField] public TeamData _blueTeamData;
    [SerializeField] private TeamData _redTeamData;
    public List<GamePlayer> _gamePlayers;
    
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

    public void AddScore(TeamData teamData, int addScore, int addPackage)
    {
        if (teamData == _blueTeamData)
        {
            _blueTeamData.AddScore(addScore);
            _blueTeamData.AddPackage(addPackage);
            _onBlueTeamScore.Raise(this, new int[] {_blueTeamData.TeamPoints, _blueTeamData.TeamPackages} );
        }
        else
        {
            _redTeamData.AddScore(addScore);
            _redTeamData.AddPackage(addPackage);
            _onRedTeamScore.Raise(this, new int[] { _redTeamData.TeamPoints, _redTeamData.TeamPackages } );
        }
    }

    private void OnDrawGizmos()
    {
        if (_blueTeamData.SpawnPoints.IsEmpty())
            return;

        foreach (Vector3 spawnPoint in _blueTeamData.SpawnPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(spawnPoint, new Vector3(0.5f, 0.5f, 0.5f));
        }
        
        if (_redTeamData.SpawnPoints.IsEmpty())
            return;
        
        foreach (Vector3 spawnPoint in _redTeamData.SpawnPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(spawnPoint, new Vector3(0.5f, 0.5f, 0.5f));
        }
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
    public int TeamPackages => _teamPackages;

    #endregion
    
    [SerializeField] private string _teamName;
    [SerializeField] private int _teamPoints;
    [SerializeField] private int _teamPackages;
    [SerializeField] private List<Vector3> _spawnPoints;
    [SerializeField] private List<GamePlayer> _teamPlayers = new List<GamePlayer>();
    
    public void AddPlayer(GamePlayer gamePlayer) 
        => _teamPlayers.Add(gamePlayer);
    public void RemovePlayer(GamePlayer gamePlayer) 
        => _teamPlayers.Remove(gamePlayer);
    public bool ContainsPlayer(GamePlayer gamePlayer) 
        => _teamPlayers.Contains(gamePlayer);
    public void AddSpawn(Vector3 spawnPosition) 
        => _spawnPoints.Add(spawnPosition);
    public void AddScore(int score) 
        => _teamPoints += score;
    public void RemoveScore(int score) 
        => _teamPoints -= score;
    public void AddPackage(int package) 
        => _teamPackages += package;
    public void RemovePackage(int package) 
        => _teamPackages -= package;

    public Vector3 GetRandomSpawnPoint()
    {
        return _spawnPoints.IsEmpty() ? new Vector3(0, 0, 0) 
            : _spawnPoints[Random.Range(0, _spawnPoints.Count)];
    }
    
    
}
