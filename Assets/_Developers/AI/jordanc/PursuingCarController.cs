using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class PursuingCarController : AICarController
{
    protected enum State { PURSUE, PATROL, ATTACK, FLEE, PICKUP, SEARCHING, DELIVERY, TURNLEFT, TURNRIGHT, BRAKE }
    [SerializeField] protected State NextState;
    public CollisionPrevention PreventionCollision;
    public GameObject MoveTarget;
    public GameObject ShootTarget;

    [SerializeField] private EntitySpawner packageSpawner;
    [SerializeField] private PackageSystem PackageSystem;
    [SerializeField] private HealthSystem Health;

    public GameObject[] AllObjects;
    public Vector3 SpawnPoint;
    public GameObject NearBank;
    public GameObject NearPackage;
    float Distance;
    float NearestDistance = 10000;

    [SerializeField] private float distanceToReset = 50f;
    [SerializeField] private float distanceBetweenAgent = 30;

    [SerializeField] private float AttackRange;

    [Header("Aggro Range")]
    [SerializeField] float AggroRange;
    [SerializeField] LayerMask Car;

    [Header("Patrol Points")]
    [SerializeField] Transform[] ListOfPatrolPoints;
    int NextPatrolPoint;

    [SerializeField] Transform SpawnZonePoint;
    [SerializeField] float DistanceFromPatrolPoint;

    [SerializeField] private Weapon weaponHandler;

    [Viewable] private Transform DeliveryPoint;
    
    protected override void Start()
    {
        if (AIDirector.Instance)
        {
            AIDirector.Instance.bots.Add(this);

            int numOfDeliveryZones = AIDirector.Instance.deliveryZones.Count;

            if (numOfDeliveryZones > 0)
                DeliveryPoint = AIDirector.Instance.deliveryZones[Random.Range(0, numOfDeliveryZones)].t;
            else
                Debug.LogWarning("No delivery points allocated in AI Director.");
        }
        else Debug.LogWarning("No AI Director found in scene.");

        AllObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

        SpawnPoint = transform.position;

        base.Start();
    }

    private void OnDestroy()
    {
        if (AIDirector.Instance)
        {
            AIDirector.Instance.bots.Remove(this);
        }

    }

    protected override void Evaluate()
    {
        newState = false;
        ShootTarget = null;

        RaycastHit[] Hits = Physics.SphereCastAll(transform.position, AggroRange, Vector3.forward, 0, Car);

        if (Hits.Length > 0)
        {
            foreach (RaycastHit hit in Hits)
            {
                if (hit.transform.gameObject != gameObject && hit.transform.CompareTag("Player"))
                {

                    ShootTarget = hit.transform.gameObject;

                }
            }

            if (MoveTarget == null && NextState != State.PICKUP)
            {
                NextState = State.PATROL;
            }
        }

        if (MoveTarget != null)
        {
            if (ShootTarget)
            {
                // Pursue
                if (Vector3.Distance(agent.transform.position, ShootTarget.transform.position) <= AggroRange)
                {
                    NextState = State.PURSUE;
                }
                else
                {
                }
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
                MoveTarget = null;
            }

            // Searching

            // IF target == NULL || need pickup

        }

        if (packageSpawner)
        {
            if (packageSpawner.SpawnedObjects.Count > 0)
            {
                NextState = State.PICKUP;
            }
        }

        if (PackageSystem.PackageAmount == PackageSystem.MaxPackages)
        {
            NextState = State.DELIVERY;
        }


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

        if (Health.HealthPercentage <= 0.3f)
        {
            NextState = State.FLEE;
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
                CourseCorrect();
                break;
        }
    }

    protected override void Act()
    {
        if (!(NextState == State.TURNLEFT || NextState == State.TURNRIGHT || NextState == State.BRAKE))
        {
            FollowAgent();
        }

        if (Vector3.Distance(transform.position, agent.transform.position) > distanceBetweenAgent * 1.2f)
        {
            agent.transform.position = transform.position;
        }

        if (Vector3.Distance(transform.position, agent.transform.position) > distanceBetweenAgent)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        State c = NextState;


        Evaluate();       

        if (c != NextState) newState = true;

        
        if (ShootTarget)
        {

            // Attack
            if (Vector3.Distance(transform.position, ShootTarget.transform.position) <= AttackRange)
            {
                if (ShootTarget.TryGetComponent<HealthSystem>(out HealthSystem hs))
                {
                    Attack();
                }
            }

        }

        if (ShootTarget)
        {
            if (Vector3.Distance(agent.transform.position, ShootTarget.transform.position) <= 15)
            {
                ShootTarget = null;
            }
        }

        SwapState();



    }

    private void Pursue()
    {
        if (ShootTarget != null)
        {
            agent.SetDestination(ShootTarget.transform.position);

        }

    }

    private void TurnLeft()
    {
        car.HandleInput(1, -1, true);
    }

    private void TurnRight()
    {
        car.HandleInput(1, 1, true);
    }

    private void Patrol()
    {
        if (ListOfPatrolPoints.Length == 0) return;

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
        if (!ShootTarget) return;
        Debug.Log("Attack");
        weaponHandler.SetAim((ShootTarget.transform.position - transform.position).normalized);
        weaponHandler.Shoot(ShootTarget.transform.position);

    }

    private void Flee()
    {
        Debug.Log("Fleeing");

        if (PackageSystem.PackageAmount >= 1)
        {
            Delivery();
        }
        else if (PackageSystem.PackageAmount == 0)
        {
            agent.SetDestination(SpawnPoint);
        }

    }

    private void Pickup()
    {
        Debug.Log("Pickup");
        
        if (newState)
        {
            MoveTarget = packageSpawner.SpawnedObjects[Random.Range(0, packageSpawner.SpawnedObjects.Count)].gameObject;
        }

        if (MoveTarget)
        {
            agent.SetDestination(MoveTarget.transform.position);
        }
        else
        {
            NextState = State.PATROL;
        }

    }

    private void Delivery()
    {
        if (AIDirector.Instance)
        {
            DeliveryPoint = AIDirector.Instance.FindClosestDeliveryZone(transform.position);
        }

        Debug.Log("Delivery");
        agent.SetDestination(DeliveryPoint.position);
    }

    private void Searching()
    {
        Debug.Log("Searching");
    }

}
