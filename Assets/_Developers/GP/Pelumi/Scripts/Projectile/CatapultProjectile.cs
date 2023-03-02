using Pelumi.Juicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JE.General;
using JE.DamageSystem;

public class CatapultProjectile : Projectile
{
    [SerializeField] private GameObject explosionParticle;
    [SerializeField] private Vector3 targetPostion;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] protected float explosionTime;

    private SphereDamager sphereDamager;
    private Coroutine moveRoutine;
    private bool hasHit;
    private bool launched;

    protected void Start()
    {
        sphereDamager = GetComponent<SphereDamager>();
    }

    public void SetUp(Vector3 targetPos,float _speed, float angle)
    {
        launched = true;
        targetPostion = targetPos;
        speed = _speed;
        moveRoutine = StartCoroutine(PathUtil.MoveObjectAlongPath(transform, transform.position, targetPostion, angle, speed, null));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!launched || !detectLayer.ContainsLayer(other.gameObject.layer) || hasHit) return;
        hasHit = true;
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        transform.SetParent(other.transform);
        OnHit();
        Debug.Log(other.transform);
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
        Debug.Log("Damage");
        sphereDamager.Damage();
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        base.DestroyProjectile();
    }
}
