using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionTime = 3f;

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
        Debug.Log("BOOM!");
        Destroy(gameObject);
    }
}
