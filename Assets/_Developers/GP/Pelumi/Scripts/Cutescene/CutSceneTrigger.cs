using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CutSceneTrigger : MonoBehaviour
{
    [SerializeField] private bool playOnAwake;
    [SerializeField] private UnityEvent OnStart;
    [SerializeField] private UnityEvent OnFinish;

    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        director.played += Director_played;
        director.stopped += Director_stopped;
    }

    private void Start()
    {
        if (director.playableAsset) director.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && director.state == PlayState.Playing)
        {
            director.Stop();
        }
    }

    private void Director_played(PlayableDirector obj)
    {
        GameMenu.Close();
        OnStart.Invoke();
    }

    private void Director_stopped(PlayableDirector obj)
    {
        GameMenu.Open();
        OnFinish.Invoke();
    }

    public void StartCutScene(PlayableAsset playableAsset = null)
    {
        if(playableAsset) director.playableAsset = playableAsset;
        if (director.playableAsset) director.Play();
    }
}
