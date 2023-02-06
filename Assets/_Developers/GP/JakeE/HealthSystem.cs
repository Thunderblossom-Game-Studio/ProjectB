using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour, IDamageable
{
    
    #region Get & Set

    public float CurrentHealth => _currentHealth;
    public float HealthPercentage => _currentHealth / _maximumHealth;
    public bool IsDead => _currentHealth <= _minimumHealth;
    
    public float MaximumHealth
    {
        get => _maximumHealth;
        set => _maximumHealth = value;
    }
    public float MinimumHealth
    {
        get => _minimumHealth;
        set => _minimumHealth = value;
    }

    #endregion

    [Viewable] [SerializeField] private bool _isImmune;
    [Viewable] [SerializeField] private float _currentHealth;

    [SerializeField] private float _spawnHealth;
    [SerializeField] private float _maximumHealth;
    [SerializeField] private float _minimumHealth;
    
    [SerializeField] private UnityEvent _onReduceHealth;
    [SerializeField] private UnityEvent _onRestoreHealth;
    [SerializeField] private UnityEvent _onDeath;
    [SerializeField] private UnityEvent _onReachFullHealth;

    public void Awake() => _currentHealth = _spawnHealth;

    public void ReduceHealth(float reduceAmount)
    {
        if (_isImmune) return;
        
        _currentHealth -= reduceAmount;
        _onReduceHealth?.Invoke();
        
        if (!(CurrentHealth <= _minimumHealth)) return;
        
        _onDeath?.Invoke();
        _currentHealth = _minimumHealth;
    }

    public void RestoreHealth(float restoreAmount)
    {
        _currentHealth += restoreAmount;
        _onRestoreHealth?.Invoke();

        if (!(CurrentHealth >= _maximumHealth)) return;
        
        _currentHealth = _maximumHealth;
        _onReachFullHealth?.Invoke();
    }

    public void RestoreHealthDuration(float restoreAmount, float duration, float tickRate)
    {
        StartCoroutine(RestoreHealthDurationRoutine(restoreAmount, duration, tickRate));
    }

    public void ReduceHealthDuration(float reduceAmount, float duration, float tickRate)
    {
        if (_isImmune) return;
        StartCoroutine(ReduceHealthDurationRoutine(reduceAmount, duration, tickRate));
    }

    public void SetImmunity(bool isImmune)
    {
        _isImmune = isImmune;
    }

    private IEnumerator ReduceHealthDurationRoutine(float reduceAmount, float duration, float tickRate)
    {
        float currentTimer = 0;
        float requiredTickRate = tickRate;
        float damagePerTick = (reduceAmount / duration) * tickRate;
        
        while (currentTimer <= duration)
        {
            yield return null;
            currentTimer += Time.deltaTime;
            if (!(currentTimer >= requiredTickRate)) continue;
            requiredTickRate += tickRate;
            ReduceHealth(damagePerTick);
        }
    }
    
    private IEnumerator RestoreHealthDurationRoutine(float reduceAmount, float duration, float tickRate)
    {
        float currentTimer = 0;
        float requiredTickRate = tickRate;
        float damagePerTick = (reduceAmount / duration) * tickRate;
        
        while (currentTimer <= duration)
        {
            yield return null;
            currentTimer += Time.deltaTime;
            if (!(currentTimer >= requiredTickRate)) continue;
            requiredTickRate += tickRate;
            RestoreHealth(damagePerTick);
        }
    }
}

public interface IDamageable
{
    void ReduceHealth(float reduceAmount);
    void RestoreHealth(float restoreAmount);
}
