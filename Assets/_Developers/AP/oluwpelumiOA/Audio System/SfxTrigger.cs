using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SfxPlayer : MonoBehaviour
{
    [Header("Group Clips")]
    [SerializeField] private GroupSfxClips[] groupSfxClips;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = AudioManager.Instance.GetSfxAudioSource();
    }

    public void PlayGroupSfx(int groupID)
    {
        GroupSfxClips groupSfx = groupSfxClips.First((x) => x.groupID == groupID);
        if(groupSfx != null) groupSfx.PlayRandomAudio(audioSource);
    }

    public void PlaySfxRandomPitch(AudioClip audioClip)
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(audioClip);
    }

    public void PlaySfx(AudioClip audioClip)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(audioClip);
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }


    [System.Serializable]
    public class GroupSfxClips
    {
        public int groupID;
        public AudioClip[] audioClips;
        public bool randomizePitch = false;
        public float pitchRange = 0.2f;

        [HideInInspector]
        public AudioClip audio;

        public void PlayRandomAudio(AudioSource audioSource)
        {
            if (randomizePitch) audioSource.pitch = Random.Range(1.0f - pitchRange, 1.0f + pitchRange);
            audio = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.PlayOneShot(audio);
        }
    }
}
