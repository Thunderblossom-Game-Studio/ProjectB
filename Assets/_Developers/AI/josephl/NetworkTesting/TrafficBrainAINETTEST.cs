using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrafficBrainAINETTEST : NetworkBehaviour
{

    [Header("Waypoint Targets")]
    [SyncVar] public Transform goal;

    private NavMeshAgent agent;

    [SyncVar] private float CarSpeed;
    float DefaultSpeed;

    // Rate limit distance checks
    [SerializeField] private float timeBetweenChecks = 0.3f;
    [SyncVar] private float timeSinceLastCheck;
    
    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        if (!base.IsServer)
            return;

        agent.destination = goal.position;
        DefaultSpeed = CarSpeed;

        TimeManager.OnTick += TimeManager_OnTick;
    }

    void TimeManager_OnTick()
    {
        if (!base.IsServer)
            return;

        if (agent.destination != goal.position)
            ReconcileDestination(this);

        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck > timeBetweenChecks)
        {
            timeSinceLastCheck = 0;
            DistanceCheck();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DistanceCheck()
    {
        Debug.Log("Distance: " + Vector3.Distance(transform.position, goal.transform.position));
        if (Vector3.Distance(transform.position, goal.transform.position) < 1)
        {
            ChangeDestination(this);
        }
    }

    [ServerRpc (RequireOwnership = false)]
    private void ChangeDestination(TrafficBrainAINETTEST car)
    {
        Debug.Log("Attempting to set destination for " + car.name);
        goal.GetComponent<WaypointControlAINETTEST>().ChangeToNextGoal(this);

        // Updating destination to goal
        car.agent.destination = car.goal.position;
    }

    [ServerRpc (RequireOwnership = false)]
    private void ReconcileDestination(TrafficBrainAINETTEST car)
    {
        car.agent.destination = car.goal.position;
    }
}
