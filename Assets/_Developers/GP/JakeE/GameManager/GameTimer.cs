using System;
using JE.Utilities;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    #region GET
    public Timer Timer => _gameTimer;
    
    #endregion

    [SerializeField] private GameEvent _onTimerUpdate;
    [SerializeField] private GameSettings _gameSettings;
    private Timer _gameTimer;

    public Action OnGameBegin;
    public Action OnGameComplete;

    private void Update()
    {
        if (_gameTimer == null) return;
        _gameTimer.Tick(Time.deltaTime);
        _onTimerUpdate.Raise(this, _gameTimer.GetRemainingTime().ToString("F2"));
    } 
    
    public void GameStart()
    {
        GameBegin();
        _gameTimer = new Timer(_gameSettings._gameDuration, GameComplete, false);
    }
    private void GameBegin() => OnGameBegin?.Invoke();
    private void GameComplete() => OnGameComplete?.Invoke();
}
