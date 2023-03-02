using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartProjectile : Projectile
{
    [SerializeField] protected float destroyTime;

    protected void Start()
    {
        StartCoroutine(LifeTimeDelay());
    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    public void SetUp(float _speed)
    {
        speed = _speed;
    }

    public void OnHit(float damageValue)
    {
        if (playerBullet) hitEvent.Raise(this, new HitMarkInfo(Color.red, transform.position));
        PopUpManager.Instance.PopUpAtTextPosition(transform.position + Vector3.up * .5f, Vector3.zero, "Hit", Color.red);
        DestroyProjectile();
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
}
