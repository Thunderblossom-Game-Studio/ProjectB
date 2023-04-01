using System;
using System.Collections;
using System.Collections.Generic;
using JE.General;
using JE.Utilities;
using UnityEngine;


namespace JE.DamageSystem
{  
    [RequireComponent(typeof(Collider))]
    public class TriggerDamager : DamageSystem
    {
        private enum DamageType
        {
            Instant,
            Overtime
        }
        
        [SerializeField] private DamageType _damageType;
        [SerializeField] private int _maxHit;

        [Viewable] [SerializeField] private int currentHit;

        private void OnTriggerEnter(Collider objectCollider)
        {
            if (!_damageLayers.ContainsLayer(objectCollider.gameObject.layer) || IsUsedUp()) return;
            switch (_damageType)
            {
                case DamageType.Instant:
                    DamageEntity(objectCollider.gameObject);
                    break;
                case DamageType.Overtime:
                    DamageEntityDuration(objectCollider.gameObject);
                    break;
            }

            if (_maxHit > uint.MinValue) ++currentHit;
        }

        public bool IsUsedUp() => _maxHit > 0 && currentHit >= _maxHit;
    }
}

