using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JE.General;
using JE.DamageSystem;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionTime = 6f;
    [SerializeField] private LayerMask instantHitLayer;
    [SerializeField] private LayerMask delayHitLayer;
    [SerializeField] private Audio3D bombAudio;
    [SerializeField] private SphereDamager damager;
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
        exploded = true;
        damager.Damage();
        bombAudio.PlaySoundEffect("BombNoise");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision hit)
    {
        if (collisionWithPlayer || timerStart) return;
        if (!instantHitLayer.ContainsLayer(hit.gameObject.layer))
        {
            collisionWithPlayer = true;
            Explosion();
            return;
        }

        if (!delayHitLayer.ContainsLayer(hit.gameObject.layer))
        {
            timerStart = true;
            bombAudio.PlaySoundEffect("BombTick");
        }
    }
}