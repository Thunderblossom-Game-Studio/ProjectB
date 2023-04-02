using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class Sandstorm : MonoBehaviour
{
    [SerializeField] private GameObject SandstormOverlay;
    [SerializeField] private float secondsToSandstorm;
    [SerializeField] private float minimumRangeOnX;
    [SerializeField] private float maximumRangeOnX;
    [SerializeField] private float minimumRangeOnZ;
    [SerializeField] private float maximumRangeOnZ;
    [SerializeField] private Text countdownTimerText;
    private float currentYpos;

    private void Start()
    {
        StartCoroutine(Timer());
        currentYpos = transform.position.y;
        SandstormOverlay.SetActive(false);
    }

    IEnumerator Timer()
    {
        float currentTime = secondsToSandstorm;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            countdownTimerText.text = "Sandstorm incoming!\n " + currentTime.ToString("N0");
            yield return null;
        }
        SandstormMove();
    }

    private void SandstormMove()
    {
        SandstormOverlay.SetActive(true);
        transform.position = transform.position = new Vector3(UnityEngine.Random.Range(minimumRangeOnX, maximumRangeOnX), currentYpos, UnityEngine.Random.Range(minimumRangeOnZ, maximumRangeOnZ));
        StartCoroutine(Timer());
    }
}

