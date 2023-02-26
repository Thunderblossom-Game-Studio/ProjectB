using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blimp : MonoBehaviour
{
    [SerializeField] private GameObject droppableObject;
    [SerializeField] private GameObject spawnIndicator;
    private GameObject spawnIndicatorInst;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float dropRate = 0.1f;
    [SerializeField] private RouteUser ru;

    private float interpolateAmount;

    private bool blimpIsMoving;
    private bool playerDetected;
    private float nextTimeToAttack = 6f;

    [SerializeField] private float detectionHeight;

    private bool indicatorActive;

    private void Start()
    {
        ru.Activate(0);
        blimpIsMoving = true;
        playerDetected = false;
    }

    private void Update()
    {
        CheckForPlayer();
    }

    private void FixedUpdate()
    {
        int layerMask = 1 << 3; //"Car" layer
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);

            playerDetected = true;
            blimpIsMoving = false;

            if(indicatorActive == false)
            {
                ActivateIndicator();
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * detectionHeight, Color.blue);
            playerDetected = false;
            blimpIsMoving = true;
            ru.ToggleMovement(true);

            if(indicatorActive == true)
            {
                DeactivateIndicator();
            }
            
        }
    }

    private void CheckForPlayer()
    {
       if(playerDetected)
        {

            if (Time.time > nextTimeToAttack)
            {
                ru.Activate(0, false);
                nextTimeToAttack = Time.time + 1 / dropRate;
                DropBomb();
            }
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
        indicatorActive = true;
    }

    private void DeactivateIndicator()
    {
        spawnIndicator.SetActive(false);
        indicatorActive = false;
    }
}
