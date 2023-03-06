using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteUser : MonoBehaviour
{
    public enum MoveType {Loop, Restart}

    [Header("Configuration")]
    [SerializeField] private bool moveOnRoute = true;
    [SerializeField] private bool rotateOnRoute = true;
    [SerializeField] private MoveType moveType;

    [Header("Properties")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Routes routes;
    [Viewable] [SerializeField] private Vector3 nextTrainRoute;

    private int currentRouteIndex = 0;
    public float MoveSpeed => moveSpeed;

    public void Activate(int startIndex, bool canMove = true, bool canRotate = true)
    {
        currentRouteIndex = startIndex;
        nextTrainRoute = routes.GetRoute(currentRouteIndex);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
    }

    public void HandleRouteChange()
    {
        if (ReachedNextRoute())
        {
            switch (moveType)
            {
                case MoveType.Loop:
                    currentRouteIndex = currentRouteIndex == routes.GetRoutes.Count - 1 ? 0 : ++currentRouteIndex;
                    break;
                case MoveType.Restart:
                    if(currentRouteIndex == routes.GetRoutes.Count - 1)
                    {
                        currentRouteIndex = 0;
                        transform.position = routes.GetRoutes[currentRouteIndex];
                    }
                    else ++currentRouteIndex;
                    break;
                default:
                    break;
            }

            nextTrainRoute = routes.GetRoute(currentRouteIndex);
        }
    }

    public bool ReachedNextRoute() => (Vector3.Distance(transform.position, nextTrainRoute) <= 0.2f);

    public void ToggleMovement(bool state) => moveOnRoute = state;

    public void ToggleRotation(bool state) => rotateOnRoute = state;
}
