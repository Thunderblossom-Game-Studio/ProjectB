using System;
using System.Collections;
using UnityEngine;


public class GameStateManager : Singleton<GameStateManager>
{
    public event Action OnStart;
    public event Action OnComplete;
    
    [SerializeField] private bool _runOnAwake;


    private void Start()
    {   
        if (_runOnAwake)
            Begin();
    }

    public void Begin()
    {
        GameTimer.Instance.OnGameComplete += StartCompleteState; 
        StartBeginState();
    }

    private IEnumerator StartState()
    {
        yield return GameSequencer.Instance.CountDownSequence();
        GameTeamManager.Instance.InitialiseTeams();
        GameTimer.Instance.GameStart();
        OnStart?.Invoke();
    }

    private IEnumerator CompleteState()
    {
        TeamData winningTeam =  GameTeamManager.Instance.GetWinningTeam();
        yield return GameSequencer.Instance.CompleteGameSequence(winningTeam);
        OnComplete?.Invoke();
    }
    
    private void StartCompleteState() => StartCoroutine(CompleteState());
    private void StartBeginState() => StartCoroutine(StartState());
}

