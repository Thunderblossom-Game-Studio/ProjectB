using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionHandler : MonoBehaviour
{
    private AICarController carHandler;
    private PackageSystem PackageSystem;

    [SerializeField] private EntitySpawner packageSpawner;
    [Viewable] private Transform DeliveryPoint;

    // Start is called before the first frame update
    private void Start()
    {
        carHandler = GetComponent<AICarController>();
        PackageSystem = GetComponent<PackageSystem>();

        if (AIDirector.Instance)
        {
            int numOfDeliveryZones = AIDirector.Instance.deliveryZones.Count;

            if (numOfDeliveryZones > 0)
                DeliveryPoint = AIDirector.Instance.deliveryZones[Random.Range(0, numOfDeliveryZones)].t;
            else
                Debug.LogWarning("No delivery points allocated in AI Director.");
        }
    }

    public AIPlayerHandler.CurrentState Evaluate(AIPlayerHandler.CurrentState state)
    {
        if (packageSpawner && packageSpawner.SpawnedObjects.Count > 0)
        {
            state = AIPlayerHandler.CurrentState.PICKUP;
        }

        if (PackageSystem.PackageAmount == PackageSystem.MaxPackages)
        {
            state = AIPlayerHandler.CurrentState.DELIVERY;
        }

        return state;
    }

    /// <summary>
    /// This method is the code for the pickup state. It finds a random package on the map and then sets the agent destination to that package.
    /// </summary>
    public AIPlayerHandler.CurrentState Pickup(bool newState, AIPlayerHandler.CurrentState state)
    {
        if ((newState || !carHandler.MoveTarget) && packageSpawner.SpawnedObjects.Count > 0)
        {
            int num = 0;
            do
            {
                carHandler.MoveTarget = packageSpawner.SpawnedObjects[Random.Range(0, packageSpawner.SpawnedObjects.Count)].gameObject;                
                if (!carHandler.FindPath(carHandler.MoveTarget.transform.position))
                {
                    carHandler.MoveTarget = null;
                }
                num++;
            } while (!carHandler.MoveTarget && num < 3);
        }

        if (!carHandler.MoveTarget)
        {
            state = AIPlayerHandler.CurrentState.IDLE;
        }
        else
        {
            carHandler.SetAgentTarget(carHandler.MoveTarget.transform.position);
        }

        return state;
    }

    /// <summary>
    /// This method is the code for the delivery state. It finds the close delivery zone and then sets the agents destination to that delivery zone 
    /// </summary>
    public void Delivery()
    {
        if (AIDirector.Instance)
        {
            DeliveryPoint = AIDirector.Instance.FindClosestDeliveryZone(transform.position);
        }

        carHandler.SetAgentTarget(DeliveryPoint.position);
    }
}
