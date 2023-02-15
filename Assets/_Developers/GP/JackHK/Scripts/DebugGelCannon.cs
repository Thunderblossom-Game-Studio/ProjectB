using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGelCannon : MonoBehaviour
{
    public GelSystem _gelSystem;
    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public bool shootCheck = true;
    public bool shootGel = false;
    public float cooldown = 1.0f;
    public float dropletForce = 30f;

    private void Awake()
    {
        if (_gelSystem == null)
        {
            _gelSystem = FindObjectOfType<GelSystem>();
        }
    }

    private void Start()
    {
        particle1.Stop();
        particle2.Stop();
        shootCheck = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            particle1.Play();
            particle2.Play();
            shootGel = true;
}
        if (Input.GetKey(KeyCode.E))
        {
            particle1.Stop();
            particle2.Stop();
            shootGel = false;
        }
        Cooldown();
    }

    void Cooldown()
    {
        if (shootCheck == false || shootGel == false)
        {
            return;
        }

        _gelSystem.CreateDroplet(gameObject.transform, Vector2.down, dropletForce);

        StartCoroutine(StartCooldown());
    }

    public IEnumerator StartCooldown()
    {
        shootCheck = false;
        yield return new WaitForSeconds(cooldown);
        shootCheck = true;
    }
}
