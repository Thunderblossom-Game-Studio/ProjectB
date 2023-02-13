using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioBank musicBank;
    [SerializeField] AudioBank sfxBank;
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource sfxPlayer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        ChangeMusicWithFade(GetMusicClip("Menu"), true);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            ChangeMusicWithFade(GetMusicClip("Menu"), true);
        }
    }

    private void ChangeMusic(AudioClip audioClip, bool loop)
    {
        musicPlayer.clip = audioClip;
        musicPlayer.loop = loop;
        musicPlayer.Play();
    }

    private IEnumerator PlayMusicFade(AudioClip audioClip, bool loop = true)
    {
        musicPlayer.clip = audioClip;
        musicPlayer.volume = 0;
        musicPlayer.loop = loop;
        musicPlayer.Play();
        while (musicPlayer.volume < 1)
        {
            musicPlayer.volume += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator StopMusicFade()
    {
        float speed = 0.05f;

        while (musicPlayer.volume >= speed)
        {
            musicPlayer.volume -= speed;
            yield return new WaitForSeconds(0.1f);
        }

        musicPlayer.Stop();
    }

    private IEnumerator ChangeMusicWithFadeRoutine(AudioClip audioClip, bool loop, float speed)
    {
        while (musicPlayer.volume >= speed)
        {
            musicPlayer.volume -= speed;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        musicPlayer.clip = audioClip;
        musicPlayer.loop = loop;
        musicPlayer.Play();

        while (musicPlayer.volume < 1)
        {
            musicPlayer.volume += speed;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private IEnumerator BlendTwoMusicRoutine(AudioClip intro, AudioClip loopMusic)
    {
        ChangeMusic(intro, false);
        yield return new WaitForSecondsRealtime(musicPlayer.clip.length - 0.5f);
        ChangeMusic(loopMusic, true);
    }
    public static AudioClip GetMusicClip(string ID)
    {
        if (!InstanceExists()) return null;
        return  Instance.musicBank.GetAudioByID(ID);
    } 
    
    public static AudioClip GetSfxClip(string ID)
    {
        if (!InstanceExists()) return null;
        return Instance.sfxBank.GetAudioByID(ID);
    }

    public AudioSource GetSfxAudioSource() => sfxPlayer;

    public static void PlaySfx(string ID, bool randomPitch = false)
    {
        if (Instance == null) return;
        Instance.sfxPlayer.pitch = randomPitch ? Random.Range(0.8f, 1.2f) : 1;
        Instance.sfxPlayer.PlayOneShot(GetSfxClip(ID));
    }

    public static void PlayMusic(string ID, bool loop = true)
    {
        if (!InstanceExists()) return;
        Instance.ChangeMusic(GetMusicClip(ID), loop);
    }

    public static void PauseMusic()
    {
        if (!InstanceExists()) return;
        Instance.musicPlayer.Pause();
    }

    public static void ResumeMusic()
    {
        if (!InstanceExists()) return;
        Instance.musicPlayer.UnPause();
    }

    public static void StopMusic()
    {
        if (!InstanceExists()) return;
        Instance.StartCoroutine(Instance.StopMusicFade());
    }

    public static void ChangeMusicWithFade(AudioClip audioClip, bool loop, float speed = 0.05f)
    {
        if (!InstanceExists()) return;
        Instance.StartCoroutine(Instance.ChangeMusicWithFadeRoutine(audioClip, loop, speed));
    }

    public static void BlendTwoMusic(AudioClip audioClip, AudioClip loopMusic)
    {
        if (!InstanceExists()) return;
        Instance.StartCoroutine(Instance.BlendTwoMusicRoutine(audioClip, loopMusic));
    }

    private static bool InstanceExists()
    {
        if (Instance == null) Debug.LogError("No Audio Manager in the scene");
        return Instance != null;
    }
}
