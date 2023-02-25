using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSequencer : MonoBehaviour
{
    [SerializeField] private GameEvent _countDownSequenceUI;

    public void CountDownSequence() => StartCoroutine(CountDownSequenceRoutine());
    public void GameLoseSequence() => StartCoroutine(CountDownSequenceRoutine());
    public void GameWinSequence() => StartCoroutine(CountDownSequenceRoutine());
    
    private IEnumerator CountDownSequenceRoutine()
    {
        GameUtilities.PauseGame();
        yield return new WaitForSeconds(1);
        _countDownSequenceUI.Raise(this, "Game Starting In 3..");
        Debug.Log("Game Starting In 3..");
        yield return new WaitForSeconds(1);
        Debug.Log("Game Starting In 2..");
        yield return new WaitForSeconds(1);
        Debug.Log("Game Starting In 1..");
        yield return new WaitForSeconds(1);
        Debug.Log("Gooooo!");
        GameUtilities.ResumeGame();
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
