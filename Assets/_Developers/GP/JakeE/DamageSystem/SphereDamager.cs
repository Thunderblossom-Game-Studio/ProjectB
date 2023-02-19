using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JE.DamageSystem
{
    public class SphereDamager : DamageSystem, IDamager
    {
        [SerializeField] private Color _debugColor = Color.white;
        [SerializeField] private DamageSphere[] _damageSpheres;
        [SerializeField] private int _hitEntitiesMaximum = 10;
        
        private Collider[] _hitEntities;
        
        private void Awake() => _hitEntities = new Collider[_hitEntitiesMaximum];
    
        public void Damage()
        {
            foreach (var damageSphere in _damageSpheres)
            {
                int entitiesHit = Physics.OverlapSphereNonAlloc(transform.position + damageSphere.SphereOffset, damageSphere.SphereRadius, _hitEntities, _damageLayers);
                for (int i = 0; i < entitiesHit; i++) DamageEntity(_hitEntities[i].gameObject);
            }
        }
    
        public void DamageDuration()
        {
            foreach (var damageSphere in _damageSpheres)
            {
                int entitiesHit = Physics.OverlapSphereNonAlloc(transform.position + damageSphere.SphereOffset, damageSphere.SphereRadius, _hitEntities, _damageLayers);
                for (int i = 0; i < entitiesHit; i++) DamageEntity(_hitEntities[i].gameObject);
            }
        }
    
    #if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (_damageSpheres.Length <= 0) return;
            foreach (var damageSphere in _damageSpheres)
            {
                Gizmos.color = _debugColor;
                Gizmos.DrawWireSphere(transform.position + damageSphere.SphereOffset, damageSphere.SphereRadius);
            }
        }
    #endif
        
    }
    
    [Serializable]
    public class DamageSphere
    {
        #region Get & Set
    
        public Vector3 SphereOffset => _sphereOffset;
        public float SphereRadius => _sphereRadius;
    
        #endregion
        
        [SerializeField] private Vector3 _sphereOffset;
        [SerializeField] private float _sphereRadius;
    }
}
