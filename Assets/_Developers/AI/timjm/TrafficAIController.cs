using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficAIController : AICarController
{
    [SerializeField] private bool CanMove = true;

    [SerializeField] private float sight = 4f;

    [SerializeField] private LayerMask cars;

    [SerializeField] private float distanceToReset = 50f;

    private int lookOffset = 1;

    internal TrafficDirector td;

    protected override void FollowAgent()
    {
        agent.gameObject.SetActive(CanMove);

        if (CanMove) base.FollowAgent();
        //else car.Accelerate(0);
    }

    protected override void Evaluate()
    {
        CanMove = true;

        if (Vector3.Distance(transform.position, agent.transform.position) > distanceToReset)
        {
            Vector3 agentAbove = agent.transform.position;

            agentAbove.y += 2;

            transform.position = agentAbove;
        }

        if (td)
        {
            if (td.Exits.Count == 0 || td.red)
            {
                CanMove = false;
            }
        }

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, .5f, Vector3.forward, sight, cars);

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject != gameObject && Vector3.Distance(transform.position, hit.transform.position) < 4.25f)
                {
                    CanMove = false;
                }
            }
        }
    }
}
