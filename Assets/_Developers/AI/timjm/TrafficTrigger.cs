using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrafficTrigger : MonoBehaviour
{
    public GameObject Target;

    public NavMeshAgent agent;

    public TrafficAIController trafficAIController;

    public float speed = 1.0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Target)
        {
            Debug.LogWarning("No target for traffic");
            return;
        }

        agent.SetDestination(Target.transform.position);

        if (Vector3.Distance(transform.position, Target.transform.position) < 1)
        {
            Target.GetComponent<TrafficDirector>().Car = this.gameObject;
            trafficAIController.td = Target.GetComponent<TrafficDirector>();
            Target.GetComponent<TrafficDirector>().Lane();
        }
    }
}
