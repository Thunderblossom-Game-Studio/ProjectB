
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrafficBrainAINETTEST : MonoBehaviour
{

    [Header("Waypoint Targets")]
    public Transform goal;

    private NavMeshAgent agent;

    private float CarSpeed;
    float DefaultSpeed;

    // Rate limit distance checks
    [SerializeField] private float timeBetweenChecks = 0.3f;
    private float timeSinceLastCheck;
    
    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void OnStartServer()
    {
        agent.destination = goal.position;
        DefaultSpeed = CarSpeed;
    }

    void TimeManager_OnTick()
    {
        if (agent.destination != goal.position)
            ReconcileDestination(this);

        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck > timeBetweenChecks)
        {
            timeSinceLastCheck = 0;
            DistanceCheck();
        }
    }

    private void DistanceCheck()
    {
        //Debug.Log("Distance: " + Vector3.Distance(transform.position, goal.transform.position));
        if (Vector3.Distance(transform.position, goal.transform.position) < 1)
        {
            ChangeDestination(this);
        }
    }

    private void ChangeDestination(TrafficBrainAINETTEST car)
    {
        //Debug.Log("Attempting to set destination for " + car.name);
        goal.GetComponent<WaypointControlAINETTEST>().ChangeToNextGoal(this);

        // Updating destination to goal
        car.agent.destination = car.goal.position;
    }

    private void ReconcileDestination(TrafficBrainAINETTEST car)
    {
        car.agent.destination = car.goal.position;
    }
}
