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
    private Vector3 startPos;
    private float t;
    private float duration;
    private Vector3 midPoint;

    protected void Start()
    {
        sphereDamager = GetComponent<SphereDamager>();
    }

    private void Update()
    {
        if (!launched || hasHit) return;
        MoveObjectAlongPath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!launched || !detectLayer.ContainsLayer(other.gameObject.layer) || hasHit) return;
        hasHit = true;
        if (moveRoutine != null) StopCoroutine(moveRoutine);

        transform.position = other.ClosestPoint(transform.position);
        transform.SetParent(other.transform);
        OnHit();
        Debug.Log(other.transform);
    }

    public void SetUp(Vector3 targetPos, float _speed, float angle)
    {
        targetPostion = targetPos;
        speed = _speed;
        float dist = Vector3.Distance(transform.position, targetPostion);
        duration = dist / speed;
        t = 0.0f;
        startPos = transform.position;
        midPoint = startPos + (targetPostion - startPos) / 2 + Vector3.up * angle;
        launched = true;
      //  moveRoutine = StartCoroutine(PathUtil.MoveObjectAlongPath(transform, transform.position, targetPostion, angle, speed, null));
    }

    public void DetectTargets()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log("Point of contact: " + hit.point);
        }
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

    public void MoveObjectAlongPath()
    {
        if (t < duration)
        {
            t += Time.deltaTime;
            float frac = t / duration;
            Vector3 start = Vector3.Lerp(startPos, midPoint, frac);
            Vector3 end = Vector3.Lerp(midPoint, targetPostion, frac);
            transform.position = Vector3.Lerp(start, end, frac);
            transform.rotation = Quaternion.LookRotation(end - start, Vector3.up);
        }
        else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
