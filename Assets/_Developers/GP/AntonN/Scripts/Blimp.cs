using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blimp : MonoBehaviour
{
    [SerializeField] private GameObject droppableObject;
    [SerializeField] private float dropRate;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private Transform pointC;
    [SerializeField] private Transform pointD;
    [SerializeField] private Transform pointABCD;

    private float interpolateAmount;
    private bool isMoving;
    private bool playerDetected;

    private void Start()
    {
        isMoving = true;
    }
    
    private void Update()
    {
        //interpolateAmount = (interpolateAmount + Time.deltaTime) % 1f;
        //pointABCD.position = CubicLerp(pointA.position, pointB.position, pointC.position, pointD.position, interpolateAmount);

        CheckForPlayer();

        if(Input.GetKeyDown(KeyCode.P))
        {
            DropBomb();
        }
    }

    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
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
    }

    private void CheckForPlayer()
    {

    }

    private void SpawnIndicator()
    {

    }
    
    private void DropBomb()
    {
        

        isMoving = false;

    }
}
