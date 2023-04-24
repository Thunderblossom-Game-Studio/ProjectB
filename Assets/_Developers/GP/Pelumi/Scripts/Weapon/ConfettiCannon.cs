
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiCannon : Weapon
{
    [SerializeField] private int bulletPerShot;
    [SerializeField] private float bulletSpread;
    [SerializeField] private float virtualBulletSize = .2f;
    [SerializeField] private LineRenderer hitEffectLinePrefab;
    [SerializeField] private List<LineRenderer> hitEffectLines;
    [SerializeField] private Collider[] _hitEntities;
    [SerializeField] private LayerMask damageLayer;

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < bulletPerShot; i++)
        {
            hitEffectLines.Add(Instantiate(hitEffectLinePrefab, firePoint[0]));
        }
    }
    public override void ShootProjectile(Vector3 targetPos, Action OnFireSuccess = null)
    {
        base.ShootProjectile(targetPos, OnFireSuccess);
        Shoot(targetPos);
    }
    private void Shoot(Vector3 targetPos)
    {
        for (int i = 0; i < bulletPerShot; i++)
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * bulletSpread;
            Vector3 firePosition = targetPos + new Vector3(randomCircle.x, randomCircle.y, 0);
            StartCoroutine(DebugTrail(firePoint[0], hitEffectLines[i], firePosition));
        }
        ModifyAmmo(currentAmmo - 1);
        fireParticle.Play();
    }

    public IEnumerator DebugTrail(Transform firePoint, LineRenderer hitEffectLine , Vector3 targetPos)
    {
        Vector3 localTargetPos = firePoint.worldToLocalMatrix.MultiplyPoint(targetPos);
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
