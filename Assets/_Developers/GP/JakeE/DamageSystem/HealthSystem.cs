using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace JE.DamageSystem
{
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

    [SerializeField] private UnityEvent<float> _onHealthChange;
    [SerializeField] private UnityEvent _onReduceHealth;
    [SerializeField] private UnityEvent _onRestoreHealth;
    [SerializeField] private UnityEvent _onDeath;
    [SerializeField] private UnityEvent _onReachFullHealth;

    private IEnumerator _reduceHealthRoutine;
    private IEnumerator _restoreHealthRoutine;

    public void Awake()
    {
        _currentHealth = _spawnHealth;
        _onDeath.AddListener(Respawn);
    } 
        
    public void ReduceHealth(float reduceAmount)
    {
        if (_isImmune) return;

        _currentHealth -= reduceAmount;

        _onReduceHealth?.Invoke();

       _onHealthChange?.Invoke(_currentHealth / _maximumHealth);

       if (!(_currentHealth <= _minimumHealth)) return;
        
        _currentHealth = _minimumHealth;
        _onDeath?.Invoke();

        if (_reduceHealthRoutine == null) return;
        StopCoroutine(_reduceHealthRoutine);
    }

    public void RestoreHealth(float restoreAmount)
    {
        _currentHealth += restoreAmount;
        _onRestoreHealth?.Invoke();

        _onHealthChange?.Invoke(_currentHealth / _maximumHealth);

        if (!(_currentHealth >= _maximumHealth)) return;
        
        _currentHealth = _maximumHealth;
        _onReachFullHealth?.Invoke();
        
        if (_restoreHealthRoutine == null) return;
        StopCoroutine(_restoreHealthRoutine);
    }

    public void RestoreHealthDuration(float restoreAmount, float duration, float tickRate)
    {
        _restoreHealthRoutine = ReduceHealthDurationRoutine(restoreAmount, duration, tickRate);
        StartCoroutine(_restoreHealthRoutine);
    }

    public void ReduceHealthDuration(float reduceAmount, float duration, float tickRate)
    {
        if (_isImmune) return;
        _reduceHealthRoutine = ReduceHealthDurationRoutine(reduceAmount, duration, tickRate);
        StartCoroutine(_reduceHealthRoutine);
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
    
    private void Respawn()
    {
        if (!gameObject.TryGetComponent(out GamePlayer gamePlayer))
            return;

        transform.position = gamePlayer.PlayerTeamData.GetRandomSpawnPoint();
        transform.rotation = Quaternion.identity;
        _currentHealth = _maximumHealth;
        
        if (!gameObject.TryGetComponent(out Rigidbody currentBody)) 
            return;
        
        currentBody.velocity = Vector3.zero;
    }

    }

public interface IDamageable
{
    void ReduceHealth(float reduceAmount);
    void RestoreHealth(float restoreAmount);
    void RestoreHealthDuration(float restoreAmount, float duration, float tickRate);
    void ReduceHealthDuration(float reduceAmount, float duration, float tickRate);
}

}




