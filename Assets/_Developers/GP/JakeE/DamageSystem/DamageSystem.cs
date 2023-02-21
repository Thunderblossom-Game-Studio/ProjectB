using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


namespace JE.DamageSystem
{
    public class DamageSystem : MonoBehaviour
    {
        #region Get & Set

        public float DamageAmount => _damageAmount;
        private float CriticalDamage => _damageAmount * _criticalDamagePercentage * Random.Range(0.8f, 1.0f);
        private bool IsCritical => !(Random.Range(0, 100) >= _criticalChance);

        #endregion
    
        [SerializeField] protected LayerMask _damageLayers;
        [SerializeField] protected float _damageAmount;
        [SerializeField] protected float _criticalDamagePercentage;
        [Range(0, 100)] [SerializeField] protected float _criticalChance;
        [Range(0, 100)] [SerializeField] protected float _duration;
        [Range(0, 100)] [SerializeField] protected float _tickRate;
    
        private Action _onDamage;

        protected void DamageEntity(GameObject damageObject)
        {
            IDamageable healthSystem = damageObject.GetComponent<IDamageable>();
            if (healthSystem == null) return;
            healthSystem.ReduceHealth(IsCritical ? CriticalDamage : _damageAmount);
            _onDamage?.Invoke();
        }

        protected void DamageEntityDuration(GameObject damageObject)
        {
            IDamageable healthSystem = damageObject.GetComponent<IDamageable>();
            if (healthSystem == null) return;
            healthSystem.ReduceHealthDuration(IsCritical ? CriticalDamage : _damageAmount, _duration, _tickRate);
            _onDamage?.Invoke();
        }
    }

    public interface IDamager
    {
        void Damage();
        void DamageDuration();
    }   
}