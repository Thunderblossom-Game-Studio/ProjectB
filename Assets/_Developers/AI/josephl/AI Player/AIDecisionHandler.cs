using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionHandler : MonoBehaviour
{
    private AICarController _carHandler;
    private PackageSystem _packageSystem;

    [Viewable] private Transform _deliveryPoint;

    private EntitySpawner _packageSpawner;
    private int _numOfAttempts = 3;

    // Start is called before the first frame update
    private void Start()
    {
        _carHandler = GetComponent<AICarController>();
        _packageSystem = GetComponent<PackageSystem>();

        if (AIDirector.Instance)
        {
            int numOfDeliveryZones = AIDirector.Instance.deliveryZones.Count;

            if (numOfDeliveryZones > 0)
                _deliveryPoint = AIDirector.Instance.deliveryZones[Random.Range(0, numOfDeliveryZones)].t;
            else
                Debug.LogWarning("No delivery points allocated in AI Director.");

            if (AIDirector.Instance.packageSpawners.Count > 0)
                _packageSpawner = AIDirector.Instance.packageSpawners[Random.Range(0, AIDirector.Instance.packageSpawners.Count)];
            else
                Debug.LogWarning("No package spawners allocated in AI Director.");
        }
    }

    public AIPlayerHandler.CurrentState Evaluate(AIPlayerHandler.CurrentState state)
    {
        if (!_packageSpawner) return state = AIPlayerHandler.CurrentState.IDLE;

        if (_packageSpawner && _packageSpawner.SpawnedObjects.Count > 0)
        {
            state = AIPlayerHandler.CurrentState.PICKUP;
        }

        if (_packageSystem.PackageAmount == _packageSystem.MaxPackages)
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
        if (!_packageSpawner) return state = AIPlayerHandler.CurrentState.IDLE;

        if ((newState || !_carHandler.MoveTarget) && _packageSpawner.SpawnedObjects.Count > 0)
        {
            int num = 0;
            do
            {
                _carHandler.MoveTarget = _packageSpawner.SpawnedObjects[Random.Range(0, _packageSpawner.SpawnedObjects.Count)].gameObject;                
                if (!_carHandler.FindPath(_carHandler.MoveTarget.transform.position))
                {
                    _carHandler.MoveTarget = null;
                }
                num++;
            } while (!_carHandler.MoveTarget && num < _numOfAttempts);
        }

        if (!_carHandler.MoveTarget)
        {
            state = AIPlayerHandler.CurrentState.IDLE;
        }
        else
        {
            _carHandler.SetAgentTarget(_carHandler.MoveTarget.transform.position);
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
            _deliveryPoint = AIDirector.Instance.FindClosestDeliveryZone(transform.position);
        }

        _carHandler.SetAgentTarget(_deliveryPoint.position);
    }
}
