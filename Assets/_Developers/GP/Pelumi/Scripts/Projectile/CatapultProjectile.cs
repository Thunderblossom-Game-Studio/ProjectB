using Pelumi.Juicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JE.General;
using JE.DamageSystem;
using System;

public class CatapultProjectile : Projectile
{
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] protected float explosionTime;

    private Vector3 targetPostion;
    private SphereDamager sphereDamager;
    private Coroutine moveRoutine;
    private bool hasHit;
    private bool launched;

    protected void Start()
    {
        sphereDamager = GetComponent<SphereDamager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!launched || !detectLayer.ContainsLayer(other.gameObject.layer) || hasHit) return;
        hasHit = true;
        if (moveRoutine != null) StopCoroutine(moveRoutine);

        transform.position = other.ClosestPoint(transform.position);
        transform.SetParent(other.transform);
        OnHit();
    }

    public void SetUp(Vector3 targetPos, float _speed, float angle)
    {
        targetPostion = targetPos;
        speed = _speed;
        launched = true;
        moveRoutine = StartCoroutine(PathUtil.MoveObjectAlongPath(transform, transform.position, targetPostion, angle, speed, null));
    }

    public void OnHit()
    {
        if (playerBullet) hitEvent.Raise(this, new HitMarkInfo(Color.red, transform.position));
        StartCoroutine(ExplosionTimeDelay());
    }

    protected IEnumerator ExplosionTimeDelay()
    {
        yield return new WaitForSeconds(explosionTime);
        DestroyProjectile();
    }

    protected override void DestroyProjectile()
    {
        sphereDamager.Damage();
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        base.DestroyProjectile();
    }
}
