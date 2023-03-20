using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHealthFX : MonoBehaviour
{
    [Header("Add the desired low health image here")]
    [SerializeField] private Image lowHealthImage = null;

    [Header("Image that flashes when hurt")]
    [SerializeField] private Image flashImage = null;
    [SerializeField] private float flashTimer = 0.1f;

    [SerializeField] private HealthSystem HealthSystem;

    public void UpdateHUD()
    {
        Color lowHealthImageAlpha = lowHealthImage.color;
        lowHealthImageAlpha.a = 1 - (HealthSystem.CurrentHealth / HealthSystem.MaximumHealth);
        lowHealthImage.color = lowHealthImageAlpha;
    }

    IEnumerator HurtFlash()
    {
        flashImage.enabled = true;
        //AudioManager.PlaySoundEffect("");
        yield return new WaitForSeconds(flashTimer);
        flashImage.enabled = false;
    }

    public void DamageTaken()
    {
        if (HealthSystem.CurrentHealth >= HealthSystem.MinimumHealth)
        {
            StartCoroutine(HurtFlash());
            UpdateHUD();

        }
    }

    //For Debugging.
    /* private void Update()
     {
         if (Input.GetKeyDown(KeyCode.F))
         {
             HealthSystem.ReduceHealth(10);

         }

         if (Input.GetKeyDown(KeyCode.V))
         {
             HealthSystem.RestoreHealth(10);
         }
     }*/
}
