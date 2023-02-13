using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioBank musicBank;
    [SerializeField] AudioBank soundEffectBank;
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource soundEffectPlayer;

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
        PlayMusic("Menu", true);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            ChangeMusicWithFade("Menu", true);
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

    public static void ChangeMusicWithFade(string audioID, bool loop = true, float speed = 0.05f)
    {
        if (!InstanceExists()) return;
        Instance.StartCoroutine(Instance.ChangeMusicWithFadeRoutine(GetMusicClip(audioID), loop, speed));
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
