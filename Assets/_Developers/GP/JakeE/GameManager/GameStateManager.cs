using System;
using System.Collections;
using System.Collections.Generic;
using JE.Utilities;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    private TeamManager _teamManager;
    private GameTimer _gameTimer;

    public event Action OnStart;
    public event Action OnUpdate;
    public event Action OnComplete;
    

    protected override void Awake()
    {
        base.Awake();
        _gameTimer.GetComponent<GameTimer>();
        _teamManager.GetComponent<TeamManager>();
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartState();
        yield return UpdateState();
        yield return CompleteState();
    }

    private IEnumerator StartState()
    {
        OnStart?.Invoke();
        _gameTimer.GameStart();
        yield return null;
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
}

