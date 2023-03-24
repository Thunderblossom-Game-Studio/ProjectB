using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JE.General;
using JE.DamageSystem;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionTime = 6f;
    [SerializeField] private LayerMask delayHitLayer;
    [SerializeField] private LayerMask instantHitLayer;
    [SerializeField] private Audio3D bombAudio;
    [SerializeField] private SphereDamager damager;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float explosionForce = 1f;
    private float timer;
    private bool exploded;
    private bool collisionWithPlayer;
    private bool timerStart;

    private void Start()
    {
        exploded = false;
        collisionWithPlayer = false;
        timerStart = false;
        timer = explosionTime;
    }

    private void Update()
    {
       
        if ((timer <= 0f && exploded == false) || (collisionWithPlayer == true))
        {
            Explosion();
        }

        if(timerStart == true)
        {
            timer -= Time.deltaTime;
        }
    }

    private void Explosion()
    {
        bombAudio.PlaySoundEffect("BombNoise");
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            damager.Damage();
        }
        exploded = true;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (collisionWithPlayer || timerStart) return;
        if (!delayHitLayer.ContainsLayer(hit.gameObject.layer))
        {
            collisionWithPlayer = true;
            Explosion();
            return;
        }

        if (!instantHitLayer.ContainsLayer(hit.gameObject.layer))
        {
            timerStart = true;
            bombAudio.PlaySoundEffect("BombTick");
        }
    }
}