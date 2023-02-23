using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameEvent hitEvent;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        hitEvent.Raise(this, new HitMarkInfo(Color.red, collision.GetContact(0).point));
        PopUpManager.Instance.PopUpAtTextPosition(collision.GetContact(0).point + Vector3.up * .5f, Vector3.zero, "Hit", Color.red);
        Destroy(gameObject);
    }
}
