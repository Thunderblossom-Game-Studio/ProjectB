using System;
using System.Collections;
using System.Collections.Generic;
using JE.DamageSystem;
using UnityEngine;

[RequireComponent(typeof(IDamager))]
public class AutoDamager : MonoBehaviour
{
    enum DamageType
    {
        Instant,
        Duration
    }

    [SerializeField] private DamageType _damageType;
    [SerializeField] private float _tickRate;
    [Viewable] [SerializeField] private bool _enabled;

    private IEnumerator _autoDamageRoutine;
    private IDamager _damager;

    private void Awake()
    {
        _damager = GetComponent<IDamager>();
        _autoDamageRoutine = DamageRoutine();
        StartDamager();
    }

    public void StartDamager()
    {
        if (_autoDamageRoutine == null) return;
        StartCoroutine(_autoDamageRoutine);
        _enabled = true;
    } 

    public void StopDamager()
    {
        if (_autoDamageRoutine == null) return;
        StopCoroutine(_autoDamageRoutine);
        _enabled = false;
    } 

    private IEnumerator DamageRoutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(_tickRate);
            switch (_damageType)
            {
                case DamageType.Instant:
                    _damager.Damage();
                    break;
                case DamageType.Duration:
                    _damager.DamageDuration();
                    break;
            }
        }
    }
}
