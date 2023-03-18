using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class ShrinkingZone : MonoBehaviour
{
    [SerializeField] private float numberOfWaves;
    [SerializeField] private float secondsToNextWave;
    [SerializeField] private float waveChangeDelay;
    [SerializeField] private Text countdownTimerText;
    [SerializeField] private GameObject zone;
    private float shrinkingSpeed = 1f;
    private Vector3 scalingRate = new Vector3(0f, 0f, 0f);
    private float timer;
    private int currentWaves;
    private bool checkLastWave;
    private float currentTime;
    private bool cooldownBool;
    private float displayWaves;
    private float zoneXpos;
    private float zoneYpos;
    private float zoneZpos;
    private bool scaleCheck;

    private void Start()
    {
        currentWaves = 0;
        checkLastWave = false;
        cooldownBool = false;
        scaleCheck = false;
        scalingRate = new Vector3(shrinkingSpeed, shrinkingSpeed, 0f);
     }

    private void Update()
    {
        if (currentWaves <= numberOfWaves)
        {
            Timer();
            Shrinking();
        }
        else if(!checkLastWave)
        {
            checkLastWave = true;
        }
    }

    private void Timer()
    {
        if (timer >= secondsToNextWave)
        {
            timer = 0;
            cooldownBool = true;
            StartCoroutine(CooldownBetweenShrinks());
            ShrinkCheck();
        }
        else if (cooldownBool == false)
        {
            timer += Time.deltaTime;
            currentTime = secondsToNextWave - timer;
            displayWaves = currentWaves + 1;
            if(checkLastWave == false)
            {
                countdownTimerText.text = "Next wave in: " + currentTime.ToString("0") + " seconds";
            }
            if (countdownTimerText.text == "Next wave in: " + 0.ToString() + " seconds")
            {
                if(currentWaves == numberOfWaves - 1)
                {
                    countdownTimerText.text = "FINAL WAVE!";
                    checkLastWave = true;
                }
                else if(currentWaves < numberOfWaves)
                {
                    countdownTimerText.text = "WAVE " + displayWaves + "\n The zone is shrinking!";
                }
            }
        }
    }

    IEnumerator CooldownBetweenShrinks()
    {
        yield return new WaitForSeconds(waveChangeDelay);
        cooldownBool = false;
    }

    private void Shrinking()
    {
        if (((scaleCheck == true) && (zone.transform.localScale.x != zoneXpos / 2) && (zone.transform.localScale.y != zoneYpos / 2)))
        {
            zone.transform.localScale -= scalingRate;
        }
        else if ((zone.transform.localScale.x <= zoneXpos / 2) && (zone.transform.localScale.y <= zoneYpos / 2))
        {
            scaleCheck = false;
        }
    }

    private void ShrinkCheck()
    {
        scaleCheck = true;
        currentWaves++;

        zoneXpos = zone.transform.localScale.x;
        zoneYpos = zone.transform.localScale.y;
        zoneZpos = zone.transform.localScale.z;
    }
}
