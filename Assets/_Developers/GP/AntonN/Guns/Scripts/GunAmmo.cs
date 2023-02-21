using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAmmo : MonoBehaviour
{
    [SerializeField] GunInfo ScriptableObject;
    [SerializeField] private GameObject ammoDecal;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, ScriptableObject.projectileRange);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, ScriptableObject.projectileSpeed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < .01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
            ContactPoint contact = other.GetContact(0);
            //GameObject.Instantiate(ammoDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
            Destroy(gameObject);
            DealDamage();
    }

    public void DealDamage()
    {
        float damage = ScriptableObject.projectileDamage;
        //Deal damage here
    }
}
