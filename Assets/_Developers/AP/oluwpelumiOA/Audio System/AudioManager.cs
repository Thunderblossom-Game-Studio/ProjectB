using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioBank musicBank;
    [SerializeField] private AudioBank soundEffectBank;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource soundEffectPlayer;

    [Header("Radio System")]
    [SerializeField] private AudioAlbum[] audioAlbums;
    [Viewable] [SerializeField] private AudioAlbum audioAlbum;
    [Viewable] [SerializeField] private int albumIndex;
    [Viewable] [SerializeField] bool fadingMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if(InputManager.Instance.HandleInteractInput().WasPressedThisFrame() && !fadingMusic && audioAlbum != null)
        {
            audioAlbum.Switch(ref albumIndex);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneType loadedScene = (SceneType)scene.buildIndex;
        PlayLevelMusic(loadedScene);
        SetMusicAlbum(loadedScene);
    }

    public void PlayLevelMusic(SceneType loadedScene)
    {
        switch (loadedScene)
        {
            case SceneType.MainMenu:
                ChangeMusicWithFade("MMTheme", true);
                break;
            case SceneType.Credits:
                ChangeMusicWithFade("MMTheme", true);
                break;
            case SceneType.Tutorial: ChangeMusicWithFade("TuneT", true); break;
            case SceneType.TempLevel1: ChangeMusicWithFade("TuneL1", true); break;
            case SceneType.TempLevel2: ChangeMusicWithFade("TuneL2", true); break;
            case SceneType.TempLevel3: ChangeMusicWithFade("TuneL2", true); break;
            case SceneType.TempLevel4: ChangeMusicWithFade("TuneL2", true); break;
            default: break;
        }
    }

    public void SetMusicAlbum(SceneType loadedScene)
    {
        audioAlbum = audioAlbums[(int)loadedScene];
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
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    private IEnumerator StopMusicFade()
    {
        float speed = 0.05f;

        while (musicPlayer.volume >= speed)
        {
            musicPlayer.volume -= speed;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        musicPlayer.Stop();
    }

    private IEnumerator ChangeMusicWithFadeRoutine(AudioClip audioClip, bool loop)
    {
        fadingMusic = true;

        while (musicPlayer.volume > 0)
        {
            musicPlayer.volume -= 0.05f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        musicPlayer.clip = audioClip;
        musicPlayer.loop = loop;
        musicPlayer.Play();

        while (musicPlayer.volume < 1)
        {
            musicPlayer.volume += 0.05f;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        fadingMusic = false;
    }

    private IEnumerator BlendTwoMusicRoutine(AudioClip intro, AudioClip loopMusic, bool loop  = true)
    {
        ChangeMusic(intro, false);
        yield return new WaitForSecondsRealtime(musicPlayer.clip.length - 0.5f);
        ChangeMusic(loopMusic, loop);
    }

    public static AudioClip GetMusicClip(string audioID)
    {
        if (!InstanceExists()) return null;
        return  Instance.musicBank.GetAudioByID(audioID);
    } 
    
    public static AudioClip GetSoundEffectClip(string audioID)
    {
        if (!InstanceExists()) return null;
        return Instance.soundEffectBank.GetAudioByID(audioID);
    }

    public AudioSource GetSfxAudioSource() => soundEffectPlayer;

    public static void PlaySoundEffect(string audioID, bool randomPitch = false)
    {
        if (Instance == null) return;
        Instance.soundEffectPlayer.pitch = randomPitch ? Random.Range(0.8f, 1.2f) : 1;
        Instance.soundEffectPlayer.PlayOneShot(GetSoundEffectClip(audioID));
    }

    public static void PlayMusic(string ID, bool loop = true)
    {
        if (!InstanceExists()) return;
        Instance.StartCoroutine(Instance.PlayMusicFade(GetMusicClip(ID), loop));
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

    public static void ChangeMusicWithFade(string audioID, bool loop = true)
    {
        if (!InstanceExists()) return;
        Instance.StartCoroutine(Instance.ChangeMusicWithFadeRoutine(GetMusicClip(audioID), loop));
    }

    public static void BlendTwoMusic(string startAudioID, string nextAudioID, bool loop = true)
    {
        if (!InstanceExists()) return;
        Instance.StartCoroutine(Instance.BlendTwoMusicRoutine(GetMusicClip(startAudioID), GetMusicClip(nextAudioID), loop));
    }

    private static bool InstanceExists()
    {
        if (Instance == null) Debug.LogError("No Audio Manager in the scene");
        return Instance != null;
    }
}
