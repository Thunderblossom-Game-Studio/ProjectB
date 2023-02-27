using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameEvent hitEvent;
    [SerializeField] private GameObject testParticle;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(DestoryOnDelay());
    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    public void OnHit(float damageValue)
    {
        hitEvent.Raise(this, new HitMarkInfo(Color.red,transform.position));
        PopUpManager.Instance.PopUpAtTextPosition(transform.position + Vector3.up * .5f, Vector3.zero, "Hit", Color.red);
        DestroyProjectile();
    }

    private IEnumerator DestoryOnDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void DestroyProjectile()
    {
        Instantiate(testParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
