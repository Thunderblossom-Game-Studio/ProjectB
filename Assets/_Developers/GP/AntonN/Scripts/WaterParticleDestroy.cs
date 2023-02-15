using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticleDestroy: MonoBehaviour
{
    [SerializeField] private GameObject ammoDecal;
    private float speed = 50f;
    private float timeToDestroy = 5f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < .01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 7)
        {
            ContactPoint contact = col.GetContact(0);
            GameObject.Instantiate(ammoDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
            Destroy(gameObject);
        }
    }
}