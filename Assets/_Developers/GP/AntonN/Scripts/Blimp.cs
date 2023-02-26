using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blimp : MonoBehaviour
{
    [SerializeField] private GameObject droppableObject;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private float dropRate = 0.1f;
    //[SerializeField] private Transform pointA;
    //[SerializeField] private Transform pointB;
    //[SerializeField] private Transform pointC;
    //[SerializeField] private Transform pointD;
    //[SerializeField] private Transform pointABCD;

    private float interpolateAmount;
    private bool blimpIsMoving;
    private bool playerDetected;
    private float nextTimeToAttack = 2;
    private bool indicatorSwitch;

    [SerializeField] private float detectionHeight;

    private void Start()
    {
        blimpIsMoving = true;
        playerDetected = false;
        indicatorSwitch = false;
    }

    private void Update()
    {
        //interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;
        //pointABCD.position = CubicLerp(pointA.position, pointB.position, pointC.position, pointD.position, interpolateAmount);

        CheckForPlayer();

        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnIndicator();
        }
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
        }
    }

    /*private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, interpolateAmount);
    }

    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);
        return Vector3.Lerp(ab_bc, bc_cd, interpolateAmount);
    }*/

    private void CheckForPlayer()
    {
       if(playerDetected)
        {
            
            
            if (Time.time > nextTimeToAttack)
            {
                nextTimeToAttack = Time.time + 1 / dropRate;
                SpawnIndicator();
                
            }
        }
    }

    private void SpawnIndicator()
    {
        
        
        DropBomb();
    }
    
    private void DropBomb()
    {
       Debug.Log("Bomb!");
       Instantiate(droppableObject, dropPoint.position, Quaternion.identity);
    }
}
