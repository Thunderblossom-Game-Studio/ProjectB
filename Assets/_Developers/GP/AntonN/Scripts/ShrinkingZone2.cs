using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingZone2 : MonoBehaviour
{
    /*[Serializable]
    public class Wave
    {
        public int waveNumber;
        public Vector3 waveSize;
        public float speed;

    }

    [SerializeField] List<Wave> waves = new List<Wave>();

    //[SerializeField] private float numberOfWaves;
    [SerializeField] private float secondsToNextWave;
    [SerializeField] private float waveChangeDelay;
    [SerializeField] private Text countdownTimerText;
    [SerializeField] private GameObject zoneBorder;
    //[SerializeField] private float shrinkingSpeed = 0.2f;
    [SerializeField] private float scaleFactor = 1.5f;
    //private float storedShrinkingSpeed;
    private Vector3 scalingRate = new Vector3(0f, 0f, 0f);
    private float timer;
    private int currentWaveIndex;
    /*private bool checkLastWave;
    private float currentTime;
    private bool cooldownBool;
    private float displayWaves;
    private float zoneXscale;
    private float zoneYscale;
    private bool readyToShrink;
    private Wave currentWave;
    private bool isShrinking;

    //private int randomX;
    //private int randomZ;

    private void Start()
    {
        isShrinking = false;
        currentWave = waves[currentWaveIndex];
    }

    private void Update()
    {
        if (currentWaves <= waves.Count)
        {
            Timer();
            Shrinking();
        }
        else if (!checkLastWave)
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
            SetCurrentWave();
            if (checkLastWave == false)
            {
                countdownTimerText.text = "Next wave in: " + currentTime.ToString("0") + " seconds";
            }
            if (countdownTimerText.text == "Next wave in: " + 0.ToString() + " seconds")
            {
                if (currentWaves == waves.Count - 1)
                {
                    countdownTimerText.text = "FINAL WAVE!";
                    checkLastWave = true;
                }
                else if (currentWaves < waves.Count)
                {
                    countdownTimerText.text = "WAVE " + currentWave.waveNumber + "\n The zone is shrinking!";
                }
            }
        }
    }

    private void SetCurrentWave()
    {
        displayWaves = currentWaves + 1;

    }

    IEnumerator CooldownBetweenShrinks()
    {
        yield return new WaitForSeconds(waveChangeDelay);
        cooldownBool = false;
    }

    private void Shrinking()
    {
        isShrinking = true;
        if ((zoneBorder.transform.localScale.x <= zoneXscale / scaleFactor) && (zoneBorder.transform.localScale.y <= zoneYscale / scaleFactor))
        {
            shrinkingSpeed = 0f;
            readyToShrink = false;
        }
        else if (readyToShrink)
        {
            zoneBorder.transform.localScale += scalingRate;
        }
    }

    private void ShrinkCheck()
    {
        readyToShrink = true;
        shrinkingSpeed = storedShrinkingSpeed;
        currentWaves++;
        //zoneBorder.transform.localPosition = zoneBorder.transform.localPosition + new Vector3(Random.Range(2, 5), Random.Range(2, 5), 0);

        zoneXscale = zoneBorder.transform.localScale.x;
        zoneYscale = zoneBorder.transform.localScale.y;
    }*/
}
