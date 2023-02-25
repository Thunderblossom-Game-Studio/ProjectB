using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSequencer : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private GameEvent _countDownSequenceUI;

    public void CountDownSequence() => StartCoroutine(CountDownSequenceRoutine());
    public void GameLoseSequence() => StartCoroutine(CountDownSequenceRoutine());
    public void GameWinSequence() => StartCoroutine(CountDownSequenceRoutine());
    public void GameCompleteSequence(TeamData winningTeam) => StartCoroutine(CompleteGameSequenceRoutine(winningTeam));
    
    private IEnumerator CountDownSequenceRoutine()
    {
        GameUtilities.PauseGame();
        for (int i = 0; i < _gameSettings._beginSequenceCount; i++)
        {
            //_countDownSequenceUI.Raise(this, $"Game Starting In {i}...");
            Debug.Log( $"Game Starting In {i+1}");
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Game is Starting!!");
        GameUtilities.ResumeGame();
    }

    private IEnumerator CompleteGameSequenceRoutine(TeamData winningTeam)
    {
        GameUtilities.SlowMotion(true);
        Debug.Log("Game Has Ended!");
        Debug.Log($"Winning Team {winningTeam.TeamName}");
        yield return new WaitForSeconds(3);
        GameUtilities.SlowMotion(false);
    }

    private IEnumerator GameLoseSequenceRoutine()
    {
        yield break;
    }

    private IEnumerator GameWinSequenceRoutine()
    {
        yield break;
    }
}
