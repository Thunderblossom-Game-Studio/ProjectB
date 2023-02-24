using System;
using System.Collections;
using System.Collections.Generic;
using JE.Utilities;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [Viewable] [SerializeField] private float _debugTimer;
    private Timer _gameTimer;

    public Action OnGameBegin;
    public Action OnGameComplete;

    public void GameStart()
    {
        GameBegin();
        _gameTimer = new Timer(_gameSettings._gameDuration, GameComplete);
    }

    private void Update() => _debugTimer = _gameTimer.GetRemainingTime();

    private void GameBegin() => OnGameBegin?.Invoke();
    private void GameComplete() => OnGameComplete?.Invoke();
}
