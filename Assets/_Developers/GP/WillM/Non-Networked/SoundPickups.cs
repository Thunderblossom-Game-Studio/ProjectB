using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPickups : MonoBehaviour
{
    private enum SoundClipType { Music, Effect };

    private AudioClip SoundClip;
    [SerializeField] private string Sound_ID = "";
    [SerializeField] private SoundClipType soundClipType = SoundClipType.Effect;

    private void Start()
    {
        if (Sound_ID == "")
        {
            Debug.LogError("Please set SoundPickup audio ID on " + gameObject.name);
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch(soundClipType)
            {
                case SoundClipType.Music:
                    AudioManager.PlayMusic(Sound_ID);
                    break;

                case SoundClipType.Effect:
                    AudioManager.PlaySoundEffect(Sound_ID);
                    break;

                default:
                    break;
            }
        }

        gameObject.SetActive(false);
    }

}
