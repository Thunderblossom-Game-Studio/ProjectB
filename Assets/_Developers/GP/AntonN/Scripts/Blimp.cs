using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blimp : MonoBehaviour
{
    public enum State { Moving, Attacking, Cooldown }
    [SerializeField] private GameObject droppableObject;
    [SerializeField] private GameObject spawnIndicator;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float dropRate = 0.1f;
    [SerializeField] private RouteUser ru;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private State state;
    [SerializeField] private float cooldownTime;

    [SerializeField] private float nextTimeToAttack = 1f;

    private float timer;

    [SerializeField] private float detectionHeight;


    private void Start()
    {
        ru.Activate(0);
    }

    private void Update()
    {
        BlimpBehaviour();
    }

    private void BlimpBehaviour()
    {
        switch (state)
        {
            case State.Moving:
                if(DetectTarget())
                {
                    OnTargetDetected();
                }
                break;
            case State.Attacking:
                AttackTarget();
                break;

            case State.Cooldown:
                Cooldown();
                break;
        }
    }

    private void OnTargetDetected()
    {
        state = State.Attacking;
        ActivateIndicator();
        ru.ToggleMovement(false);
    }

    private bool DetectTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, detectLayer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);

            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * detectionHeight, Color.blue);

            return false;
        }
    }

    private void AttackTarget()
    {
            if (timer >= nextTimeToAttack)
            {
                state = State.Cooldown;
                timer = 0;
                DropBomb();
                ru.ToggleMovement(true);
            }
            else
            {
                timer += Time.deltaTime;
            }
    }
    
    private void Cooldown()
    {
        if (timer >= cooldownTime)
        {
            
            state = State.Moving;
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private void DropBomb()
    {
        DeactivateIndicator();
        Debug.Log("Bomb!");
        Instantiate(droppableObject, dropPoint.position, Quaternion.identity);
    }

    private void ActivateIndicator()
    {
        spawnIndicator.SetActive(true);
    }

    private void DeactivateIndicator()
    {
        spawnIndicator.SetActive(false);
    }
}
