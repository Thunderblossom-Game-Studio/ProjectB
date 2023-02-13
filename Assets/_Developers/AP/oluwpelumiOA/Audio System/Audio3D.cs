using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Audio3D: MonoBehaviour
{
    private AudioSource audioSource;

    private void  Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1;
    }

    public void PlaySoundEffect(string audioID)
    {
        audioSource.PlayOneShot(AudioManager.GetSoundEffectClip(audioID));
    }

    public void PlayMusic(string audioID, bool loop)
    {
        audioSource.clip = AudioManager.GetSoundEffectClip(audioID);
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetMinDistance(float minDistance)
    {
        audioSource.minDistance = minDistance;
    }

    public void SetMaxDistance(float maxDistance)
    {
        audioSource.maxDistance = maxDistance;
    }
}
