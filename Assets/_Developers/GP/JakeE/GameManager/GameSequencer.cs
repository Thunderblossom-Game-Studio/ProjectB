using System.Collections;
using UnityEngine;

public class GameSequencer : Singleton<GameSequencer>
{
    [SerializeField] private GameSettings _gameSettings;
    
    [Header("Game Events")]
    [SerializeField] private GameEvent _onCentreTextUpdate;
    [SerializeField] private GameEvent _onMenuUpdate;
    
    public IEnumerator CountDownSequence()
    {
        GameUtilities.PauseGame();
        yield return new WaitForSecondsRealtime(1);
        for (int i = 0; i < _gameSettings._beginSequenceText.Length; i++)
        {
            _onCentreTextUpdate.Raise(this, new object[] 
                {_gameSettings._beginSequenceText[i], i, _gameSettings._beginSequenceText.Length} );
            yield return new WaitForSecondsRealtime(1);
        }
        _onCentreTextUpdate.Raise(this, new object[] {"Hide", int.MaxValue, int.MaxValue});
        GameUtilities.ResumeGame();
    }

    public IEnumerator CompleteGameSequence(TeamData winningTeam)
    {
        GameUtilities.SlowMotion(true);
        for (int i = 0; i < _gameSettings._completeSequenceText.Length; i++)
        {
            _onCentreTextUpdate.Raise(this, new object[]
                {_gameSettings._completeSequenceText[i], i, _gameSettings._completeSequenceText.Length} );
            yield return new WaitForSecondsRealtime(1);
        }
        _onCentreTextUpdate.Raise(this, new object[] {"Hide", int.MaxValue, int.MaxValue});
        GameUtilities.SlowMotion(false);
        GameUtilities.PauseGame();
        
        _onMenuUpdate.Raise(this, true);
    }
}
