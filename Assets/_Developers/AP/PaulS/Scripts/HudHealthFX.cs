using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHealthFX : MonoBehaviour
{
    [Header("Drag the car here")]
    [SerializeField] private HealthSystem healthSystem;

    [Header("Add the desired low health image here")]
    [SerializeField] private Image lowHealthImage = null;

    //[Header("Image that flashes when hurt")]
    //[SerializeField] private Image flashImage = null;
    //[SerializeField] private float flashTimer = 0.1f;

    [Header("Fade related values")]
    [Tooltip("How long it takes for the effect to start fading")]
    [SerializeField] private float fadeTimer = 3f;
    [Tooltip("How long it takes for effect to fully fade")]
    [SerializeField] private float fadeDuration = 2f;

    [Header("Additional Tweaking")]
    [SerializeField] private float maxFadeTimer = 3f;
    [SerializeField] private bool startFadeTimer = false;
    [SerializeField] private bool canFade = false;

    public void UpdateHUD()
    {
        Color lowHealthImageAlpha = lowHealthImage.color;
        lowHealthImageAlpha.a = 1;
        lowHealthImage.color = lowHealthImageAlpha;
        lowHealthImage.CrossFadeAlpha(1, 0, false);
    }

    public void DamageFadeOut()
    {
        if (canFade)
        {
            lowHealthImage.CrossFadeAlpha(0, fadeDuration, false);
            canFade = false;
        }
    }

    //IEnumerator HurtFlash()
    //{
    //    flashImage.enabled = true;
    //    //AudioManager.PlaySoundEffect("");
    //    yield return new WaitForSeconds(flashTimer);
    //    flashImage.enabled = false;
    //}

    public void DamageTaken()
    {
        if (healthSystem.CurrentHealth >= healthSystem.MinimumHealth)
        {
            canFade = false;
            //StartCoroutine(HurtFlash());
            UpdateHUD();
            fadeTimer = maxFadeTimer;
            startFadeTimer = true;
        }
    }

    private void Update()
    {
        if (startFadeTimer)
        {
            fadeTimer -= Time.deltaTime;
            if (fadeTimer <= 0)
            {
                canFade = true;
                startFadeTimer = false;
            }
        }

        if (canFade)
        {
            DamageFadeOut();
        }

        ///For debugging
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    healthSystem.ReduceHealth(10);

        //}

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    healthSystem.RestoreHealth(10);
        //}
    }
}
