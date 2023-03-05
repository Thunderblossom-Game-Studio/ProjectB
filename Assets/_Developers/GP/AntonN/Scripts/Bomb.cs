using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionTime = 6f;

    private float timer;
    bool exploded = false;

    private void Start()
    {
        timer = explosionTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f && exploded == false)
        {
            Explosion();
            exploded = true;
        }
    }

    private void Explosion()
    {
        //Actual explosion logic not implemented but would go here
        Debug.Log("BOOM!");
        Destroy(gameObject);
    }
}
