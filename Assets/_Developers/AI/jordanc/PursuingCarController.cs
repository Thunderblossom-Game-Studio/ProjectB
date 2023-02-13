using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class PursuingCarController : AICarController
{
    protected enum State { PURSUE, PATROL, ATTACK, FLEE, PICKUP, SEARCHING, DELIVERY, TURNLEFT, TURNRIGHT, BRAKE }
    [SerializeField] protected State NextState;
    private AITestCar CurrentCar;
    public CollisionPrevention PreventionCollision;
    public GameObject Target;

    [SerializeField] private float distanceToReset = 50f;

    
    [SerializeField] private float AttackRange;

    [Header("Aggro Range")]
    [SerializeField] float AggroRange;
    [SerializeField] LayerMask Car;

    [Header("Patrol Points")]
    [SerializeField] Transform[] ListOfPatrolPoints;
    int NextPatrolPoint;
    [SerializeField] float DistanceFromPatrolPoint;

    protected override void Start()
    {
        base.Start();
        CurrentCar = GetComponent<AITestCar>();
    }

    protected override void Evaluate()
    {
        if (PreventionCollision.TurnLeftBoolPass == true)
        {
            NextState = State.TURNLEFT;
        }
        if (PreventionCollision.TurnRightBoolPass == true)
        {
            NextState = State.TURNRIGHT;
        }
        if (PreventionCollision.BrakeBoolPass == true)
        {
            NextState = State.BRAKE;
        }

        RaycastHit[] Hits = Physics.SphereCastAll(transform.position, AggroRange, Vector3.forward, 0, Car);

        if (Hits.Length > 0)
        {
            foreach (RaycastHit hit in Hits)
            {
                if (hit.transform.gameObject != gameObject && hit.transform.CompareTag("Player"))
                {

                    Target = hit.transform.gameObject;
                    Debug.Log(hit.transform.gameObject.name);

                }
            }


        }

        if (Target != null)
        {
            // Pursue
            if (Vector3.Distance(agent.transform.position, Target.transform.position) <= AggroRange)
            {
                NextState = State.PURSUE;
            }

            // Patrol
            else
            {
                NextState = State.PATROL;
         
            }

            // Attack
            if (Vector3.Distance(transform.position, Target.transform.position) <= AttackRange)
            {
                NextState = State.ATTACK;
            }

            // Flee

            // IF health < threshold || ammo < threshold

            // Pickup

            if (NextState == State.SEARCHING)
            {
                // Add heuristics
            }

            // Delivery

            // IF hasPackage
                // Deliever

            // Reset Target
            if (NextState == State.PATROL)
            {
                Target = null;
            }

            // Searching

            // IF target == NULL || need pickup

        }

        

    }



    protected override void SwapState()
    {

        switch (NextState)
        {
            case State.PURSUE:
                Pursue();
                break;
            case State.PATROL:
                Patrol();
                break;
            case State.ATTACK:
                Attack();
                break;
            case State.FLEE:
                Flee();
                break;
            case State.PICKUP:
                Pickup();
                break;
            case State.DELIVERY:
                Delivery();
                break;
            case State.SEARCHING:
                Searching();
                break;
            case State.TURNLEFT:
                TurnLeft();
                break;
            case State.TURNRIGHT:
                TurnRight();
                break;
            case State.BRAKE:
                Brake();
                break;
        }
    }



    protected override void Act()
    {
        if (NextState == State.PURSUE || NextState == State.PATROL)
        {
            FollowAgent();
        }

        

        State c = NextState;

        Evaluate();

       

        if (c != NextState) newState = true;

        SwapState();
    }


    private void Pursue()
    {
        if (Target != null)
        {
            agent.SetDestination(Target.transform.position);
        }

    }

    private void TurnLeft()
    {
        CurrentCar.Turn(-1);
    }

    private void TurnRight()
    {
        CurrentCar.Turn(1);
    }

    private void Brake()
    {
        CurrentCar.Accelerate(-1);
    }

    private void Patrol()
    {

        if (Vector3.Distance(transform.position, agent.transform.position) > distanceToReset)
        {
            Vector3 agentAbove = agent.transform.position;

            agentAbove.y += 2;

            transform.position = agentAbove;
        }

        float DistanceToNext = Vector3.Distance(transform.position, ListOfPatrolPoints[NextPatrolPoint].position);

        if (DistanceToNext <= DistanceFromPatrolPoint)
        {
            NextPatrolPoint += 1;

            if (NextPatrolPoint >= ListOfPatrolPoints.Length)
            {
                NextPatrolPoint = 0;
            }

        }

        agent.SetDestination(ListOfPatrolPoints[NextPatrolPoint].position);



    }

    private void Attack()
    {
        Debug.Log("Attacked");
    }

    private void Flee()
    {
        Debug.Log("Fleeing");
    }

    private void Pickup()
    {
        Debug.Log("Pickup");
    }

    private void Delivery()
    {
        Debug.Log("Pickup");
    }

    private void Searching()
    {
        Debug.Log("Pickup");
    }


    protected override void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 0, 0, 255);
        Gizmos.DrawSphere(transform.position, AggroRange);

        base.OnDrawGizmos();
    }
    */
}
