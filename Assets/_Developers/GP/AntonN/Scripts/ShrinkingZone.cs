using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class ShrinkingZone : MonoBehaviour
{
    [Serializable] public class Wave
    {
        public int waveNumber;
        public Vector3 waveSize;
        public float duration;
    }

    [SerializeField] List<Wave> waves = new List<Wave>();
    [SerializeField] private float secondsToNextWave;
    [SerializeField] private float waveChangeDelay;
    [SerializeField] private Text countdownTimerText;
    private int currentWaveIndex;
    private Wave currentWave;

    private void Start()
    {
        currentWave = waves[currentWaveIndex];
        StartCoroutine(Timer());
    }
    
    IEnumerator Timer()
    {
        float currentTime = secondsToNextWave;
        while(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            countdownTimerText.text = "WAVE " + currentWave.waveNumber + "\n The zone is shrinking! , " + currentTime;
            yield return null;
        }
        StartCoroutine(ScaleZone());
    }

    IEnumerator ScaleZone()
    {
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;
        while (elapsedTime < currentWave.duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / currentWave.duration);
            transform.localScale = Vector3.Lerp(originalScale, currentWave.waveSize, t);
            yield return null;
        }
        NextWave();
    }

    private void NextWave()
    {
        if(currentWaveIndex < waves.Count - 1)
        {
            currentWaveIndex++;
            currentWave = waves[currentWaveIndex];
            StartCoroutine(Timer());
        }
        else
        {
            WaveCompleted();
        }
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Completed");
    }
}

