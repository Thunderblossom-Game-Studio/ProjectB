using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blimp : MonoBehaviour
{
    [SerializeField] private GameObject droppableObject;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float dropRate = 0.1f;
    [SerializeField] private RouteUser ru;
    [SerializeField] private Transform target;


    private float interpolateAmount;

    private bool blimpIsMoving;
    private bool playerDetected;
    private float nextTimeToAttack = 6f;

    [SerializeField] private float detectionHeight;


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
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * detectionHeight, Color.blue);
            playerDetected = false;
            blimpIsMoving = true;
            ru.ToggleMovement(true);
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
        Debug.Log("Bomb!");
        Instantiate(droppableObject, dropPoint.position, Quaternion.identity);
    }


}
