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
    
        private void OnTriggerEnter(Collider objectCollider)
        {
            if (!_damageLayers.ContainsLayer(objectCollider.gameObject.layer)) return;
        
            switch (_damageType)
            {
                case DamageType.Instant:
                    DamageEntity(objectCollider.gameObject);
                    return;
                case DamageType.Overtime:
                    DamageEntityDuration(objectCollider.gameObject);
                    return;
            }
        }
    }
}

