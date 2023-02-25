using System;
using System.Collections;
using System.Collections.Generic;
using JE.Utilities;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    private GameTeamManager _teamManager;
    private GameTimer _gameTimer;
    private GameSequencer _gameSequencer;

    public event Action OnStart;
    public event Action OnUpdate;
    public event Action OnComplete;
    
    protected override void Awake()
    {
        base.Awake();
        _gameTimer = GetComponent<GameTimer>();
        _teamManager = GetComponent<GameTeamManager>();
        _gameSequencer = GetComponent<GameSequencer>();
    }

    private void Start()
    {
        _gameTimer.OnGameComplete += StartCompleteState; 
        StartBeginState();
    }

    private IEnumerator StartState()
    {
        _gameTimer.GameStart();
        _teamManager.InitialiseTeams();
        _gameSequencer.CountDownSequence();
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
        yield return null;
    }

    private void StartCompleteState() => StartCoroutine(CompleteState());
    private void StartBeginState() => StartCoroutine(StartState());
    private void StartUpdateState() => StartCoroutine(UpdateState());


}

