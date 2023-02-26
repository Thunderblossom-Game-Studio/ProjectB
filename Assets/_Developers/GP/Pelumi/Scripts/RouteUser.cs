using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteUser : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool moveOnRoute = true;
    [SerializeField] private bool rotateOnRoute = true;

    [Header("Properties")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Routes routes;
    [SerializeField] private Vector3 nextTrainRoute;

    public float MoveSpeed => moveSpeed;

    public void Activate(int startIndex, bool canMove = true, bool canRotate = true)
    {
        nextTrainRoute = routes.GetRoutes()[startIndex];
        moveOnRoute = canMove;
        rotateOnRoute = canRotate;
    }

    public void Update()
    {
        if (moveOnRoute) MoveToRoute();
        if (rotateOnRoute) RotateToFaceTarget();
        if (moveOnRoute || rotateOnRoute) HandleRouteChange();
    }

    public void MoveToRoute()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextTrainRoute, moveSpeed * Time.deltaTime);
    }

    public void RotateToFaceTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(transform.position - nextTrainRoute);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * moveSpeed);
    }

    public void HandleRouteChange()
    {
        if (ReachedNextRoute())
        {
            nextTrainRoute = routes.GetRoutes()[(nextTrainRoute == routes.GetRoutes()[^1]) ? 0 : routes.GetRoutes().IndexOf(nextTrainRoute) + 1];
        }
    }

    public bool ReachedNextRoute() => (Vector3.Distance(transform.position, nextTrainRoute) <= 0.2f);

    public void ToggleMovement(bool state) => moveOnRoute = state;

    public void ToggleRotation(bool state) => rotateOnRoute = state;
}
