using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingZone : MonoBehaviour
{
    [SerializeField] private float secondsToShrink;
    [SerializeField] private float amountOfWaves;
    [SerializeField] private float secondsToCooldown;
    private float timer;
    private int currentWaves;
    private bool outputTest;
    private float cooldown;

    private void Start()
    {
        currentWaves = 0;
        outputTest = false;
    }

    private void Update()
    {
        if(currentWaves <= amountOfWaves)
        {
            Timer();
        }
        else
        {
            if(!outputTest)
            {
                Debug.Log("End of match");
                outputTest = true;
            }
        }
    }

    private void Timer()
    {
        if (timer >= secondsToShrink)
        {
            timer = 0;
            ShrinkZone();
        }
        else
        {
            timer += Time.deltaTime;
            Debug.Log(secondsToShrink - timer + 1);
        }
    }

    private void CooldownBetweenShrinks()
    {
        if (cooldown >= secondsToCooldown)
        {
            cooldown = 0;
        }
        else
        {
            cooldown += Time.deltaTime;
        }
    }

    private void ShrinkZone()
    {
        currentWaves++;
        Debug.Log("Shrinking!");
        Debug.Log(currentWaves);
    }
}
