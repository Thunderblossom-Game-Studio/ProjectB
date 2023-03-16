using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio3D: MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private bool control3DSettingHere = true;

    [Header("3D Settings")]
    [Range(0, 5)] [SerializeField] private float _dopplerLevel = 0;
    [Range(0, 360)] [SerializeField] private float _spread = 0;
    [SerializeField] private AudioRolloffMode _audioRolloffMode = AudioRolloffMode.Linear;
    [Range(0, 500)] [SerializeField] private float _minDistance = 0;
    [Range(0, 1000)] [SerializeField] private float _maxDistance = 500;

    [Header("Play Settings")]
    [SerializeField] private bool playAudioOnStart;
    [SerializeField] private string musicClipID;
    [SerializeField] private bool loopAudio;

    private void  Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (playAudioOnStart) PlayMusicEffect(musicClipID, loopAudio);
    }

    public void PlaySoundEffect(string audioID, bool loop = false)
    {
        if(loop)
        {
            audioSource.clip = AudioManager.GetSoundEffectClip(audioID);
            audioSource.loop = loop;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(AudioManager.GetSoundEffectClip(audioID));
        }
    }

    public void PlayMusicEffect(string audioID, bool loop)
    {
        audioSource.clip = AudioManager.GetMusicClip(audioID);
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Resume()
    {
        audioSource.UnPause();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void SetDopplerLevel(float minDistance)
    {
        audioSource.dopplerLevel = minDistance;
    }

    public void SetSpread(float maxDistance)
    {
        audioSource.spread = maxDistance;
    }
    public void SetVolumeRollOff(AudioRolloffMode  audioRolloffMode)
    {
        audioSource.rolloffMode = audioRolloffMode;
    }

    public void SetMinDistance(float minDistance)
    {
        audioSource.minDistance = minDistance;
    }

    public void SetMaxDistance(float maxDistance)
    {
        audioSource.maxDistance = maxDistance;
    }

    private void OnDrawGizmos()
    {
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1;
        if (control3DSettingHere)
        {
            SetDopplerLevel(_dopplerLevel);
            SetSpread(_spread);
            SetVolumeRollOff(_audioRolloffMode);
            SetMinDistance(_minDistance);
            SetMaxDistance(_maxDistance);
        }
        else
        {
            _dopplerLevel = audioSource.dopplerLevel;
            _spread = audioSource.spread;
            _audioRolloffMode = audioSource.rolloffMode;
            _minDistance = audioSource.dopplerLevel;
            _maxDistance = audioSource.maxDistance;
        }
    }
}
