using JE.DamageSystem;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.AI;

public class PursuingCarController : AICarController
{
    protected enum State { PURSUE, PATROL, ATTACK, FLEE, PICKUP, DELIVERY }
    [SerializeField] protected State NextState;

    protected GameObject MoveTarget;
    protected GameObject ShootTarget;

    [Header("Systems")]
   
    [SerializeField] protected EntitySpawner packageSpawner;
    [SerializeField] protected PackageSystem PackageSystem;
    [SerializeField] protected HealthSystem Health;

    protected Vector3 SpawnPoint;

    [SerializeField] protected BackTriggerCheck frontTriggerCheck;
    [SerializeField] protected BackTriggerCheck backTriggerCheck;

    [SerializeField] protected float distanceToReset = 50f;
    [SerializeField] protected float distanceBetweenAgent = 30;

    [Header("Attack Range")]
    [Tooltip("This will change the range in which the bots can attack from")]
    [SerializeField] private float AttackRange;

    [Header("Aggro Range")]
    [Tooltip("This is the range the boss can see and chase after the player from")]
    [SerializeField] float AggroRange;
    [Tooltip("Layer which the AIBots will chase after/interact with")]
    [SerializeField] LayerMask Car;

    [Header("Patrol Points")]
    [SerializeField] Transform[] ListOfPatrolPoints;
    int NextPatrolPoint;

    [SerializeField] Transform SpawnZonePoint;
    [SerializeField] float DistanceFromPatrolPoint;

    [SerializeField] private Weapon weaponHandler;

    [Viewable] private Transform DeliveryPoint;        
    [Viewable] private float agentSpawnWeight = 1;

    [SerializeField] private GamePlayer gamePlayer;

    internal float fleeThreshold;

    public GamePlayer GetGamePlayer => gamePlayer;

    protected override void Start()
    {
        if (AIDirector.Instance)
        {
            AIDirector.Instance.bots.Add(this);

            fleeThreshold = AIDirector.Instance.tierOne.healthThreshold/100;

            int numOfDeliveryZones = AIDirector.Instance.deliveryZones.Count;

            if (numOfDeliveryZones > 0)
                DeliveryPoint = AIDirector.Instance.deliveryZones[Random.Range(0, numOfDeliveryZones)].t;
            else
                Debug.LogWarning("No delivery points allocated in AI Director.");
        }
        else Debug.LogWarning("No AI Director found in scene.");

        //weaponHandler = GetComponent<OldWeaponHandler>();

        //AllObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");

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

            // Reset Target
            if (NextState == State.PATROL)
            {
                MoveTarget = null;
            }

        }

        if (packageSpawner && packageSpawner.SpawnedObjects.Count > 0)
        {
                NextState = State.PICKUP;
        }

        if (PackageSystem.PackageAmount == PackageSystem.MaxPackages)
        {
            NextState = State.DELIVERY;
        }

        if (Health.HealthPercentage <= fleeThreshold)
        {
            NextState = State.FLEE;
        }

        //if (GameStateManager.Instance)
        //{
        //    if (GameStateManager.Instance.GameTimer.Timer.GetRemainingTime() < 20f)
        //    {
        //        NextState = State.DELIVERY;
        //    }
        //}
    }

    /// <summary>
    /// This method allows for the AIBot to swap to different states.
    /// </summary>
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
        }
    }

    protected override void Act()
    {
        FollowAgent();

        if (backTriggerCheck.active) agentSpawnWeight = 3;
        else if (frontTriggerCheck.active) agentSpawnWeight = -3;
        else agentSpawnWeight = -1;

        if (Vector3.Distance(transform.position, agent.transform.position) > distanceBetweenAgent * 1.1f)
        {
            Vector3 pos = gameObject.transform.position;// get pos
            pos += transform.forward * agentSpawnWeight; // finding behind
            agent.transform.position = pos;
        }

        if (Vector3.Distance(transform.position, agent.transform.position) > distanceBetweenAgent)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        if (agent.isStopped && car.GetSpeed() <= 5 && NextState == State.PICKUP)
        {

        }

        State next = NextState;


        Evaluate();

        if (next != NextState) newState = true;

        if (agent.isPathStale)
        {
            newState = true;
        }

        Shoot();

        SwapState();
    }

    protected virtual void Shoot()
    {
        if (ShootTarget)
        {
            if (ShootTarget.TryGetComponent<GamePlayer>(out GamePlayer gp) && gamePlayer.PlayerTeamData != null)
            {
                if (gamePlayer.PlayerTeamData.TeamName == gp.PlayerTeamData.TeamName)
                {
                    ShootTarget = null;
                }
            }
        }

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

            if (Vector3.Distance(agent.transform.position, ShootTarget.transform.position) <= 15)
            {
                ShootTarget = null;
            }

        }
    }

    private void Pursue()
    {
        if (ShootTarget != null)
        {
            agent.SetDestination(ShootTarget.transform.position);

        }

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

    /// <summary>
    /// This method is the code for the flee state. If the AIBot has packages on them it will go to a delivery zone, if not it will flee to a spawn zone.
    /// </summary>
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

    /// <summary>
    /// This method is the code for the pickup state. It finds a random package on the map and then sets the agent destination to that package.
    /// </summary>
    private void Pickup()
    {
        Debug.Log("Pickup");
        
        if ((newState || !MoveTarget) && packageSpawner.SpawnedObjects.Count > 0)
        {
            do
            {
                MoveTarget = packageSpawner.SpawnedObjects[Random.Range(0, packageSpawner.SpawnedObjects.Count)].gameObject;

                NavMeshPath path = new NavMeshPath();

                agent.CalculatePath(MoveTarget.transform.position, path);

                if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
                {
                    MoveTarget = null;
                }
            } while (!MoveTarget);
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

    /// <summary>
    /// This method is the code for the delivery state. It finds the close delivery zone and then sets the agents destination to that delivery zone 
    /// </summary>
    private void Delivery()
    {
        if (AIDirector.Instance)
        {
            DeliveryPoint = AIDirector.Instance.FindClosestDeliveryZone(transform.position);
        }

        Debug.Log("Delivery");
        agent.SetDestination(DeliveryPoint.position);
    }



}
