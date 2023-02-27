using System;
using System.Collections;
using System.Collections.Generic;
using JE.Utilities;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    public GameTeamManager TeamManager { get; private set; }
    public GameTimer GameTimer { get; private set; }
    public GameSequencer GameSequencer { get; private set; }

    public event Action OnStart;
    public event Action OnUpdate;
    public event Action OnComplete;
    
    protected override void Awake()
    {
        base.Awake();
        GameTimer = GetComponent<GameTimer>();
        TeamManager = GetComponent<GameTeamManager>();
        GameSequencer = GetComponent<GameSequencer>();
    }

    private void Start()
    {
        GameTimer.OnGameComplete += StartCompleteState; 
        StartBeginState();
    }

    private IEnumerator StartState()
    {
        TeamManager.InitialiseTeams();
        yield return GameSequencer.CountDownSequence();
        GameTimer.GameStart();
        
        OnStart?.Invoke();
        yield return UpdateState();
    }
    
    private IEnumerator UpdateState()
    {
        OnUpdate?.Invoke();
        yield return null;
    }

    private IEnumerator CompleteState()
    {
        OnComplete?.Invoke();
        TeamData winningTeam =  TeamManager.GetWinningTeam();
        GameSequencer.GameCompleteSequence(winningTeam);
        yield return null;
    }

    private void StartCompleteState() => StartCoroutine(CompleteState());
    private void StartBeginState() => StartCoroutine(StartState());
    private void StartUpdateState() => StartCoroutine(UpdateState());


}

