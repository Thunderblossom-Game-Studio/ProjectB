using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeCarSounds : MonoBehaviour
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;

    [Viewable] [SerializeField] private float currentSpeed;
    [Viewable] [SerializeField] private float pitchFromCar;

    private Rigidbody _rigidbody;
    private AudioSource _carAudio;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        EngineSound();
    }

    private void EngineSound()
    {
        currentSpeed = _rigidbody.velocity.magnitude;
        pitchFromCar = _rigidbody.velocity.magnitude / 50f;

        if(currentSpeed < minSpeed)
        {
            _carAudio.pitch = minPitch;
        }

        if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {
            _carAudio.pitch = minPitch + pitchFromCar;
        }

        if (currentSpeed > maxSpeed)
        {
            _carAudio.pitch = maxSpeed;
        }
    }
}
