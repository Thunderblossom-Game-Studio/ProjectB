
using JE.DamageSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartGun : Weapon
{
    [SerializeField] private float virtualBulletSize = .2f;
    [SerializeField] private Collider[] _hitEntities;
    [SerializeField] private LayerMask damageLayer;

    public override void ShootProjectile(Vector3 targetPos, Action OnFireSuccess = null)
    {
        base.ShootProjectile(targetPos, OnFireSuccess);
        Shoot(targetPos);
    }

    private void Shoot(Vector3 targetPos)
    {
        for (int i = 0; i < firePoint.Length; i++)
        {
            ModifyAmmo(currentAmmo - 1);
            StartCoroutine(DebugTrail(firePoint[i], targetPos));
        }
        fireParticle.Play();
    }

    public IEnumerator DebugTrail(Transform firePoint, Vector3 targetPos)
    {
        Vector3 localTargetPos = firePoint.worldToLocalMatrix.MultiplyPoint(targetPos);
        LineRenderer hitEffectLine = firePoint.GetComponentInChildren<LineRenderer>();
        hitEffectLine.SetPosition(1, localTargetPos);
        hitEffectLine.enabled = true;
        yield return new WaitForSeconds(0.01f);
        hitEffectLine.enabled = false;
        CheckHit(targetPos);
    }

    public void CheckHit(Vector3 targetPos)
    {
        _hitEntities = Physics.OverlapSphere(targetPos, virtualBulletSize, damageLayer);
        if (_hitEntities.Length != 0)
        {
            for (int i = 0; i < _hitEntities.Length; i++)
            {
                DealDamage(_hitEntities[i].gameObject, targetPos);
            }
        }
    }
}
