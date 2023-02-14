using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    //This can be adapted to something else if we decide not to use health at all

    [Header("Player Health Amount")]
    public float currentPlayerHealth = 100.0f;
    [SerializeField] private float maxPlayerHealth = 100.0f;
    [SerializeField] private int regenRate = 1;
    private bool canRegen = false;

    [Header("Add the desired image here")]
    [SerializeField] private Image redSplatterImage = null;

    [Header("Image that flashes when hurt/buffed")]
    [SerializeField] private Image flashImage = null;
    [SerializeField] private float flashTimer = 0.1f;

    [Header("Heal Timer")]
    [SerializeField] private float healCooldown = 3.0f;
    [SerializeField] private float maxHealCooldown = 3.0f;
    [SerializeField] private bool startCooldown = false;

    [Header("Audio Name")]
    [SerializeField] private AudioClip hurtAudio = null; //can be boostAudio for example if we opt out of health
    private AudioSource healthAudioSource;

    private void Start()
    {
        healthAudioSource = GetComponent<AudioSource>();
    }

    void UpdateHealth()
    {
        Color splatterAlpha = redSplatterImage.color;
        splatterAlpha.a = 1 - (currentPlayerHealth / maxPlayerHealth);
        redSplatterImage.color = splatterAlpha;
        
    }

    IEnumerator HurtFlash()
    {
        flashImage.enabled = true;
        healthAudioSource.PlayOneShot(hurtAudio);
        yield return new WaitForSeconds(flashTimer);
        flashImage.enabled = false;
    }

    public void TakeDamage()
    {
        if(currentPlayerHealth >= 0)
        {
            canRegen = false;
            StartCoroutine(HurtFlash());
            UpdateHealth();
            healCooldown = maxHealCooldown;
            startCooldown = true;
        }
    }

    private void Update()
    {
        if (startCooldown)
        {
            healCooldown -= Time.deltaTime;
            if(healCooldown <= 0)
            {
                canRegen = true;
                startCooldown = false;
            }
        }

        if (canRegen)
        {
            if(currentPlayerHealth <= maxPlayerHealth - 0.01)
            {
                currentPlayerHealth += Time.deltaTime * regenRate;
                UpdateHealth();
            }
            else
            {
                currentPlayerHealth = maxPlayerHealth;
                healCooldown = maxHealCooldown;
                canRegen = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            currentPlayerHealth -= 10;
            TakeDamage();
        }
    }
}
