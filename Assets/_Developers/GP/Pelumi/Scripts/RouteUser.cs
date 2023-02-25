using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteUser : MonoBehaviour
{ 
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Routes routes;
    [SerializeField] private Vector3 nextTrainRoute;

    public void Update()
    {
        if (!canMove) return;
        Move(moveSpeed, rotateSpeed);
    }

    public void Activate()
    {
        transform.position = routes.GetRoutes()[0];
        nextTrainRoute = routes.GetRoutes()[1];
        canMove = true;
    }

    public void Move(float moveSpeed, float rotateSpeed)
    {
        MoveToRoute(moveSpeed);
        RotateToFaceTarget(rotateSpeed);
        CheckDistanceToWayPoint();
    }

    public void MoveToRoute(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, nextTrainRoute, speed * Time.deltaTime);
    }

    public void RotateToFaceTarget(float speed)
    {
        Quaternion rotation = Quaternion.LookRotation(transform.position - nextTrainRoute);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);
    }

    public void CheckDistanceToWayPoint()
    {
        if (Vector3.Distance(transform.position, nextTrainRoute) <= 0.2f)
        {
            nextTrainRoute = routes.GetRoutes()[(nextTrainRoute == routes.GetRoutes()[^1]) ? 0 : routes.GetRoutes().IndexOf(nextTrainRoute) + 1];
        }
    }

    public void EnableMovement() => canMove = true;

    public void DisableMovement() => canMove = false;
}
