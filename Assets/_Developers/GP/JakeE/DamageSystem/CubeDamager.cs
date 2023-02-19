using System;
using UnityEngine;


namespace JE.DamageSystem
{
  public class CubeDamager : DamageSystem, IDamager
  {
      [SerializeField] protected Color _debugColor = Color.white;
      [SerializeField] private DamageBox[] _damageBoxes;
      [SerializeField] private int _hitEntitiesMaximum = 10;
      
      private Collider[] _hitEntities;
      
      private void Awake() => _hitEntities = new Collider[_hitEntitiesMaximum];
  
      public void Damage()
      {
          foreach (DamageBox damageBox in _damageBoxes)
          {
              int entitiesHit = Physics.OverlapBoxNonAlloc(transform.position + damageBox.BoxOffset, damageBox.BoxSize / 2, _hitEntities, Quaternion.identity, _damageLayers);
              for (int i = 0; i < entitiesHit; i++) DamageEntity(_hitEntities[i].gameObject);
              for (int i = 0; i < entitiesHit; i++) Debug.Log(_hitEntities[i].gameObject);
          }
      }
  
      public void DamageDuration()
      {
          foreach (DamageBox damageBox in _damageBoxes)
          {
              int entitiesHit = Physics.OverlapBoxNonAlloc(transform.position + damageBox.BoxOffset, damageBox.BoxSize / 2, _hitEntities, Quaternion.identity, _damageLayers);
              for (int i = 0; i < entitiesHit; i++) DamageEntityDuration(_hitEntities[i].gameObject);
          }
      }
  
  #if UNITY_EDITOR
      public void OnDrawGizmos()
      {
          if (_damageBoxes.Length <= 0) return;
          foreach (var damageBox in _damageBoxes)
          {
              Gizmos.color = _debugColor;
              Gizmos.DrawWireCube(transform.position + damageBox.BoxOffset, damageBox.BoxSize); 
          }
      }
  #endif
      
  }

  [Serializable]
  public class DamageBox
  {
      #region Get & Set

      public Vector3 BoxSize => _boxSize;
      public Vector3 BoxOffset => _boxOffset;

      #endregion

      [SerializeField] private Vector3 _boxSize;
      [SerializeField] private Vector3 _boxOffset;
  }
}


