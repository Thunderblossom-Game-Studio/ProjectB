using JE.DamageSystem;
using JE.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartProjectile : Projectile
{
    [SerializeField] protected float destroyTime;
    [SerializeField] protected float damageRadius;
    [SerializeField] protected float _damageAmount;
    [SerializeField] private int _hitEntitiesMaximum = 10;
    private bool hasHit = false;
    private Collider[] _hitEntities;
   

    protected void Start()
    {
        _hitEntities = new Collider[_hitEntitiesMaximum];
        StartCoroutine(LifeTimeDelay());
    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;
        DetectDamageable();
    }

    public void SetUp(float _speed)
    {
        speed = _speed;
    }

    protected IEnumerator LifeTimeDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        DestroyProjectile();
    }

    protected override void DestroyProjectile()
    {
        Instantiate(impactParticle, transform.position, Quaternion.identity);
        base.DestroyProjectile();
    }

    public void DetectDamageable()
    {
        if (hasHit) return;
        int entitiesHit = Physics.OverlapSphereNonAlloc(transform.position , damageRadius, _hitEntities, detectLayer);
       
        if(entitiesHit != 0)
        {
            hasHit = true;
            OnHit(entitiesHit);
        }
    }

    public void OnHit(int entitiesHit)
    {
        for (int i = 0; i < entitiesHit; i++) DamageEntity(_hitEntities[i].gameObject);
        DestroyProjectile();
    }

    protected void DamageEntity(GameObject damageObject)
    {
        Debug.Log(gameObject.name);
        IDamageable healthSystem = damageObject.GetComponent<IDamageable>();
        if (healthSystem == null) return;
        healthSystem.ReduceHealth(_damageAmount);
        OnDamage(_damageAmount);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
