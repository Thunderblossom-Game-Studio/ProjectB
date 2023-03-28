using System;
using System.Collections;


public class GameStateManager : Singleton<GameStateManager>
{
    public event Action OnStart;
    //public event Action OnUpdate;
    public event Action OnComplete;

    private void Start()
    {
        GameTimer.Instance.OnGameComplete += StartCompleteState; 
        StartBeginState();
    }

    private IEnumerator StartState()
    {
        yield return GameSequencer.Instance.CountDownSequence();
        //yield return null;
        GameTeamManager.Instance.InitialiseTeams();
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

