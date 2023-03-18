using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class ShrinkingZone : MonoBehaviour
{
    [SerializeField] private float numberOfWaves;
    [SerializeField] private float secondsToShrink;
    [SerializeField] private float delayDuringShrink;
    [SerializeField] private Text countdownTimerText;
    [SerializeField] private Transform centrePoint;
    [SerializeField] private GameObject insideZone;
    [SerializeField] private GameObject outsideZone;

    private float timer;
    private int currentWaves;
    private bool checkLastShrink;
    private float currentTime;
    private bool cooldownBool;
    private float displayWaves;

    private void Start()
    {
        currentWaves = 0;
        checkLastShrink = false;
        cooldownBool = false;
    }

    private void Update()
    {
        if(currentWaves <= numberOfWaves-1)
        {
            Timer();
        }
        else
        {
            if(!checkLastShrink)
            {
                checkLastShrink = true;
            }
        }
    }

    private void Timer()
    {
        if (timer >= secondsToShrink)
        {
            timer = 0;
            cooldownBool = true;
            StartCoroutine(CooldownBetweenShrinks());
            ShrinkZone();
        }
        else if (cooldownBool == false)
        {
            timer += Time.deltaTime;
            currentTime = secondsToShrink - timer;
            displayWaves = currentWaves + 1;
            countdownTimerText.text = "Zone will shrink in: " + currentTime.ToString("0") + " seconds";
            if (countdownTimerText.text == "Zone will shrink in: " + 0.ToString() + " seconds")
            {
                if(currentWaves+1 == numberOfWaves)
                {
                    countdownTimerText.text = "FINAL WAVE!";
                }
                else
                {
                    countdownTimerText.text = "WAVE " + displayWaves + "\n The zone is shrinking!";
                }
            }
        }
    }

    IEnumerator CooldownBetweenShrinks()
    {
        yield return new WaitForSeconds(delayDuringShrink);
        cooldownBool = false;
    }

    private void ShrinkZone()
    {
        currentWaves++;
        
    }
}
