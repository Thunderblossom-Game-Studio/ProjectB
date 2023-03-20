using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIVehicleController))]
public class AICarController : MonoBehaviour
{
    private AIVehicleController car;

    [Header("Pathfinding Settings")]

    [SerializeField] private NavMeshAgent agent;
    [Range(0f, 1f)] [SerializeField] private float forwardmultiplier;
    [Range(0f, 5f)] [SerializeField] private float turnmultiplier;
    [Range(0, 100)] [SerializeField] private float brakeSensitivity = 50;
    [Range(0, 180)] [SerializeField] private int angle;
    [SerializeField] private float stopDistance = 10;

    [SerializeField] private float defaultSpawnWeight = 5f;
    [SerializeField] private float spawnWeight = 3f;

    [SerializeField] private BackTriggerCheck frontTriggerCheck;
    [SerializeField] private BackTriggerCheck backTriggerCheck;

    [Viewable] private float agentSpawnWeight = 2;
    [SerializeField] private float defaultAgentAcc;
    [SerializeField] private float weightedAgentAcc;
    [SerializeField] private float distanceBetweenAgent = 30;

    public GameObject MoveTarget;

    private float maxIdleTimer = 4;
    private float idleTimer = 0;
    private bool stuck = false;

    // Start is called before the first frame update
    private void Start()
    {
        car = GetComponent<AIVehicleController>();
    }

    private void Update()
    {
        if (!agent)
        {
            Debug.LogWarning("No NavMesh Agent Assigned");

            return;
        }

        FollowAgent();
        CourseCorrection();
        IdleTiming();
    }

    public AIPlayerHandler.CurrentState Evaluate(AIPlayerHandler.CurrentState state)
    {
        if ((Vector3.Distance(transform.position, agent.transform.position) > distanceBetweenAgent * 1.6f)
            ||
            (frontTriggerCheck.active || backTriggerCheck.active)
            ||
            stuck)
        {
            state = AIPlayerHandler.CurrentState.IDLE;
        }
        return state;
    }

    /// <summary>
    /// Makes the car move in the direction of the navmesh agent it's chasing at all times
    /// </summary>
    public void FollowAgent()
    {
        Vector3 dir = (agent.transform.position - transform.position).normalized;

        float distance = Vector3.Distance(transform.position, agent.transform.position);

        float direction = Vector3.SignedAngle(transform.forward, dir, Vector3.up);

        float forwardInput = 0;
        float horizontalInput = 0;

        // acceleration/reversing
        if ((direction < -90) || (direction > 90))
        {
            forwardInput = -1;
        }
        else
        {
            forwardInput = 1 * forwardmultiplier;
        }

        // turning
        if (direction < -angle / 2)
        {
            horizontalInput = -1;
        }
        else if (direction > angle / 2)
        {
            horizontalInput = 1;
        }

        // braking
        bool b = false;

        if (
            // if not turning and speed is greater than brake sens
            (horizontalInput != 0 && car.GetSpeed() > brakeSensitivity) ||

            // if in stopping distance and speed is greater than brake sens
            (Vector3.Distance(transform.position, agent.transform.position) < stopDistance &&
            car.GetSpeed() > brakeSensitivity) ||

            // if speed is greater than bleh
            (car.GetSpeed() > brakeSensitivity * 1.8))
        {
            b = true;
        }

        forwardInput = Mathf.Clamp(forwardInput, -1f, 1f);
        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);

        car.HandleInput(forwardInput, horizontalInput, b);
    }

    /// <summary>
    /// State that attempts to fix a crash
    /// </summary>
    public void CourseCorrection()
    {
        if (backTriggerCheck.active) agentSpawnWeight = spawnWeight;
        else if (frontTriggerCheck.active) agentSpawnWeight = -spawnWeight;
        else agentSpawnWeight = defaultSpawnWeight;
        if (agentSpawnWeight != defaultSpawnWeight) agent.speed = weightedAgentAcc;
        else agent.speed = defaultAgentAcc;
        if (Vector3.Distance(transform.position, agent.transform.position) > distanceBetweenAgent)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
        if (Mathf.Abs(agent.transform.position.y - transform.position.y) > 5)
        {
            agent.Warp(transform.position);
        }
    }

    public void RecallAgent()
    {
        Vector3 pos = transform.position;
        pos += transform.forward * agentSpawnWeight;
        SetAgentTarget(pos);
    }

    public bool FindPath(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(target, path);
        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        }
        return true;
    }

    public bool StalePath()
    {
        return agent.isPathStale;
    }

    public void SetAgentTarget(Vector3 position)
    {
        agent.SetDestination(position);
    }

    private void IdleTiming()
    {
        if (car.GetSpeed() < 3)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer > maxIdleTimer)
            {
                stuck = true;
            }
        }
        else
        {
            stuck = false;
            idleTimer = 0f;
        }
    }

    public void OnDrawGizmos()
    {
        if (agent)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(agent.transform.position, .5f);
        }
    }
}