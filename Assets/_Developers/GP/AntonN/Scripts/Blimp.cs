using HeathenEngineering.SteamworksIntegration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] private float sphereSize;
    [SerializeField] private int dropAmount = 1;
    [SerializeField] private float raycastDistance;
    [SerializeField] private Audio3D fanAudio;
    [SerializeField] private float detectDelay = 3f;
    private float timer;
    private bool GizmosActive;
    private float delayTimer;
    private bool delayActive;
    private Vector3 hitPoint;

    private void Start()
    {
        ru.Activate(0);
        GizmosActive = false;
        delayActive = false;
        delayTimer = 0;
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
                if (DetectTarget())
                {
                    if (delayActive == false)
                    {
                        OnTargetDetected();
                        delayActive = true;
                    }
                    else if (delayActive == true)
                    {
                        DelayAfterDetect();
                        ru.ToggleMovement(true);
                        DeactivateIndicator();
                    }
                }
                else if (!DetectTarget())
                {
                    ru.ToggleMovement(true);
                    DeactivateIndicator();
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

            SpawnGround(hitPoint);

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
        fanAudio.PlaySoundEffect("BombHatch");
        DeactivateIndicator();
        for (int i = 0; i < dropAmount; i++)
        {
            Instantiate(droppableObject, dropPoint.position, Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));

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

    void SpawnGround(Vector3 endPos)
    {
        Vector3 centerPos = new Vector3(dropPoint.position.x + endPos.x, dropPoint.position.y + endPos.y) / 2;

        float scaleX = Mathf.Abs(dropPoint.position.x - endPos.x);
        float scaleY = Mathf.Abs(dropPoint.position.y - endPos.y);

        centerPos.x -= 0.5f;
        centerPos.y += 0.5f;
        spawnIndicator.transform.position = centerPos;
        spawnIndicator.transform.localScale = new Vector3(scaleX, scaleY, 5);
    }

    private void DelayAfterDetect()
    {
        if (delayTimer >= detectDelay)
        {
            delayTimer = 0;
            delayActive = false;
        }
        else
        {
            delayTimer += Time.deltaTime;
            delayActive = true;
        }
    }
}
