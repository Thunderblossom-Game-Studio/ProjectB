using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public HealthSystem healthSystem;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void Sethealth(int health)
    {
        slider.value = health;
    }

    public void UpdateHealthBar()
    {
        slider.value = healthSystem.CurrentHealth;
    }
        
}
