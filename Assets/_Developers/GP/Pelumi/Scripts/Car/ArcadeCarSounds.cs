using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeCarSounds : MonoBehaviour
{
    private float initialCarEngineSoundPitch;
    private Rigidbody _rigidbody;
    private AudioSource _carAudio;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        initialCarEngineSoundPitch = _carAudio.pitch;
    }

    private void Update()
    {
        EngineSound();
    }

    private void EngineSound()
    {
        float engineSoundPitch = initialCarEngineSoundPitch + (Mathf.Abs(_rigidbody.velocity.magnitude) / 25f);
        _carAudio.pitch = engineSoundPitch;
    }
}
