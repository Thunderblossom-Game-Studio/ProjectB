using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blimp : MonoBehaviour
{
    public enum State { Moving, Attacking, Cooldown }
    [SerializeField] private GameObject droppableObject;
    [SerializeField] private GameObject spawnIndicator;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private RouteUser ru;
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private State state;
    [SerializeField] private float secondsToDropBomb = 1f;
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private float detectionHeight = 100f;
    [SerializeField] private float boxSize = 20f;
    private float timer;
    private bool GizmosActive;
    [SerializeField] private int dropAmount = 1;
    RaycastHit hit;

    private void Start()
    {
        ru.Activate(0);
        GizmosActive = false;
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
                else if(!DetectTarget())
                {
                    ru.ToggleMovement(true);
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

    void OnDrawGizmos()
    {
        if (GizmosActive == true)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance);
            Gizmos.DrawWireCube(transform.position + transform.TransformDirection(Vector3.down) * detectionHeight, transform.localScale * boxSize);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * detectionHeight);
            Gizmos.DrawWireCube(transform.position + transform.TransformDirection(Vector3.down) * detectionHeight, transform.localScale * boxSize);
        }
    }

    private bool DetectTarget()
    {
        if(Physics.BoxCast(transform.position, transform.localScale * boxSize, transform.TransformDirection(Vector3.down), out hit, transform.rotation, detectionHeight, detectLayer))
        {
            Debug.Log("PLAYER DETECTED");
            GizmosActive = true;
            return true;
        }
        else
        {
            GizmosActive = false;
            return false;
        }
    }

    private void AttackTarget()
    {
        if (timer >= secondsToDropBomb)
        {
            state = State.Cooldown;
            timer = 0;
            DropBomb();
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
        for (int i = 0; i < dropAmount; i++)
        {
            Instantiate(droppableObject, dropPoint.position, Quaternion.Euler(new Vector3 (Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360))));
        }
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
