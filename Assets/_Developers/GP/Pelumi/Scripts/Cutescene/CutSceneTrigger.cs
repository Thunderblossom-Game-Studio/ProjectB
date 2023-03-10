using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CutSceneTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent OnStart;
    [SerializeField] private UnityEvent OnFinish;

    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_played;
        director.stopped += Director_stopped;
    }

    private void Director_played(PlayableDirector obj)
    {
        Debug.Log("OnStart");
        OnStart.Invoke();
    }

    private void Director_stopped(PlayableDirector obj)
    {
        Debug.Log("OnFinish");
        OnFinish.Invoke();
    }

    public void StartCutScene(PlayableAsset playableAsset)
    {
        Debug.Log("Start");
        director.playableAsset = playableAsset;
        director.Play();
    }
}
