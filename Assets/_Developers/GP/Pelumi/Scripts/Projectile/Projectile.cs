using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected bool playerBullet;
    [SerializeField] protected GameEvent hitEvent;
    [SerializeField] protected LayerMask detectLayer;
    [SerializeField] protected GameObject impactParticle;
    [Viewable] [SerializeField] protected float speed = 0;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void OnDamage(float damageValue)
    {
        if (playerBullet) hitEvent.Raise(this, new HitMarkInfo(Color.red, transform.position));
        PopUpManager.Instance?.PopUpAtTextPosition(transform.position + Vector3.up * .5f, Vector3.zero, damageValue.ToString(), Color.red);
    }

    protected virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
