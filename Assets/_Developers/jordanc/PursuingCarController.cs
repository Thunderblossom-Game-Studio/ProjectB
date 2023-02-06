using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuingCarController : AICarController
{
    protected enum State { PURSUE, TURNLEFT, TURNRIGHT, BRAKE, PATROL }
    [SerializeField] protected State NextState;
    private AITestCar CurrentCar;
    public CollisionPrevention PreventionCollision;
    public GameObject Target;



    [Header("Aggro Range")]
    [SerializeField] float AggroRange;
    [SerializeField] LayerMask Car;

    [Header("Patrol Points")]
    [SerializeField] Vector3[] ListOfPatrolPoints;
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

        /*
        if ((PreventionCollision.TurnLeftBoolPass == false) && (PreventionCollision.TurnRightBoolPass == false) && (PreventionCollision.BrakeBoolPass == false))
        {
            NextState = State.PURSUE;
        }
        */

        RaycastHit[] Hits = Physics.SphereCastAll(transform.position, AggroRange, Vector3.forward, 0, Car);

        if (Hits.Length > 0)
        {
            foreach (RaycastHit hit in Hits)
            {
                if (hit.transform.gameObject != gameObject && hit.transform.CompareTag("Player"))
                {
                    
                    Target = hit.transform.gameObject;


                }
            }


        }

        if (Target != null) 
        {
            if (Vector3.Distance(agent.transform.position, Target.transform.position) <= AggroRange)
            {
                NextState = State.PURSUE;
            }
            else
            {
                NextState = State.PATROL;
                Target = null;
            }
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

        /*
        if (Input.GetKeyDown(KeyCode.I) && agentDebug)
        {
            agentDebug.SetActive(!agentDebug.activeInHierarchy);
        }
        */

        if (c != NextState) newState = true;

        SwapState();
    }


    private void Pursue()
    {
        if(Target != null) 
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

        float DistanceToNext = Vector3.Distance(transform.position, ListOfPatrolPoints[NextPatrolPoint]);

        if (DistanceToNext <= DistanceFromPatrolPoint)
        {
            NextPatrolPoint += 1;

            if (NextPatrolPoint >= ListOfPatrolPoints.Length)
            {
                NextPatrolPoint = 0;
            }

        }

        agent.SetDestination(ListOfPatrolPoints[NextPatrolPoint]);



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color (255,0,0, 255);
        Gizmos.DrawSphere(transform.position, AggroRange);
    }

}
