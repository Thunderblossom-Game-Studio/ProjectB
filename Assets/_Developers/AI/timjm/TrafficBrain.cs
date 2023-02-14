using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBrain : MonoBehaviour
{
    public Transform goal;
    UnityEngine.AI.NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, goal.transform.position) < 1)
        {
            goal.GetComponent<WaypointControl>().Car = this.gameObject;
            //trafficAIController.td = Target.GetComponent<TrafficDirector>();
            goal.GetComponent<WaypointControl>().Lane();
            agent.destination = goal.position;
        }
    }
}
