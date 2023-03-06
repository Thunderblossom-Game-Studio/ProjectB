using System;
using System.Collections;
using System.Collections.Generic;
using JE.Utilities;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public event Action OnStart;
    public event Action OnUpdate;
    public event Action OnComplete;

    private void Start()
    {
        GameTimer.Instance.OnGameComplete += StartCompleteState; 
        StartBeginState();
    }

    private IEnumerator StartState()
    {
        GameTeamManager.Instance.InitialiseTeams();
        yield return GameSequencer.Instance.CountDownSequence();
        GameTimer.Instance.GameStart();
        OnStart?.Invoke();
    }

    private IEnumerator CompleteState()
    {
        OnComplete?.Invoke();
        TeamData winningTeam =  GameTeamManager.Instance.GetWinningTeam();
        GameSequencer.Instance.GameCompleteSequence(winningTeam);
        yield return null;
    }
    
    private void StartCompleteState() => StartCoroutine(CompleteState());
    private void StartBeginState() => StartCoroutine(StartState());
}

