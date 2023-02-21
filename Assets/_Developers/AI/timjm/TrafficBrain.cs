using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBrain : MonoBehaviour
{
    public Transform goal;
    public GameObject pointy;
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
            goal.GetComponent<WaypointControl>().Lane();
            agent.destination = goal.position;
        }
        RaycastHit hit;
        Vector3 forward = pointy.transform.TransformDirection(Vector3.forward) * 2;
        if (Physics.Raycast(pointy.transform.position, forward, out hit, 5.0f))
        {
            if (hit.rigidbody != null)
            {
                agent.isStopped = true;
            }
        }
        else
        {
            agent.isStopped = false;
        }
    }
}
