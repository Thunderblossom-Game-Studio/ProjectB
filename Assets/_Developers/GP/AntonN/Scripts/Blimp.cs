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
    [SerializeField] private LayerMask raycastLayer;
    [SerializeField] private State state;
    [SerializeField] private float secondsToDropBomb = 1f;
    [SerializeField] private float cooldownTime = 1f;
    [SerializeField] private float detectionHeight = 100f;
    [SerializeField] private float sphereSize;
    private float timer;
    private bool GizmosActive;
    [SerializeField] private int dropAmount = 1;
    RaycastHit hit;

    [SerializeField] private float raycastDistance;

    private Vector3 hitPoint;

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
        AudioManager.PlaySoundEffect("BlimpFan");
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
        if (GizmosActive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitPoint, sphereSize);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hitPoint, sphereSize);
        }
    }

    private bool DetectTarget()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit groundHit, raycastDistance, raycastLayer))
        {
            hitPoint = groundHit.point;
            Debug.DrawLine(transform.position, hitPoint, Color.red);

            Collider[] hitColliders = Physics.OverlapSphere(hitPoint, sphereSize, detectLayer);

            if (hitColliders.Length != 0)
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
        return false;
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
