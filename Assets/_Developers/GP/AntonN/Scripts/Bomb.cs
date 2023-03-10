using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionTime = 6f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask environmentLayer;
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
        Debug.Log("BOOM!");
        //AudioManager.PlaySoundEffect("BBExplode");

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision hit)
    {
        //if bomb directly collides with any object in "playerLayer", bomb explodes.
        if((playerLayer.value & 1 << hit.gameObject.layer) == 1 << hit.gameObject.layer)
        {
            collisionWithPlayer = true;
            Explosion();
        }
        //if bomb collides with this layer, the timer will start (any other layer, but not the bomb itself)
        else if ((environmentLayer.value & 1 << hit.gameObject.layer) == 1 << hit.gameObject.layer)
        {
            timerStart = true;
        }
    }
}