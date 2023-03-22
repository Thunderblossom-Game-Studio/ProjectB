using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIVehicleController))]
public class AICarController : NetworkBehaviour
{
    private AIVehicleController _car;

    [Header("Pathfinding Settings")]

    [SerializeField] private NavMeshAgent _agent;
    [Range(0f, 1f)] [SerializeField] private float _forwardmultiplier;
    [Range(0f, 5f)] [SerializeField] private float _turnmultiplier;
    [Range(0, 100)] [SerializeField] private float _brakeSensitivity = 50;
    [Range(0, 180)] [SerializeField] private int _angle;
    [SerializeField] private float _stopDistance = 10;

    [SerializeField] private float _defaultSpawnWeight = 5f;
    [SerializeField] private float _spawnWeight = 3f;

    [SerializeField] private BackTriggerCheck _frontTriggerCheck;
    [SerializeField] private BackTriggerCheck _backTriggerCheck;

    [Viewable] private float _agentSpawnWeight = 2;
    [SerializeField] private float _defaultAgentAcc;
    [SerializeField] private float _weightedAgentAcc;
    [SerializeField] private float _distanceBetweenAgent = 30;

    [SerializeField] private float _yWarp = 7;

    [Viewable] public GameObject MoveTarget;

    private float _maxIdleTimer = 4;
    private float _idleTimer = 0;
    private bool _stuck = false;

    private float _distanceMultiplier = 1.6f;

    // Start is called before the first frame update
    private void Start()
    {
        _car = GetComponent<AIVehicleController>();
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if (!_agent)
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
        if ((Vector3.Distance(transform.position, _agent.transform.position) > _distanceBetweenAgent * _distanceMultiplier)
            ||
            (_frontTriggerCheck.active || _backTriggerCheck.active)
            ||
            _stuck)
        {
            state = AIPlayerHandler.CurrentState.IDLE;
        }
        return state;
    }

    /// <summary>
    /// Makes the car move in the direction of the navmesh agent it's chasing at all times
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void FollowAgent()
    {
        Vector3 dir = (_agent.transform.position - transform.position).normalized;

        float distance = Vector3.Distance(transform.position, _agent.transform.position);

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
            forwardInput = 1 * _forwardmultiplier;
        }

        // turning
        if (direction < -_angle / 2)
        {
            horizontalInput = -1;
        }
        else if (direction > _angle / 2)
        {
            horizontalInput = 1;
        }

        // braking
        bool b = false;

        if (
            // if not turning and speed is greater than brake sens
            (horizontalInput != 0 && _car.GetSpeed() > _brakeSensitivity) ||

            // if in stopping distance and speed is greater than brake sens
            (Vector3.Distance(transform.position, _agent.transform.position) < _stopDistance &&
            _car.GetSpeed() > _brakeSensitivity) ||

            // if speed is greater than bleh
            (_car.GetSpeed() > _brakeSensitivity * 1.8))
        {
            b = true;
        }

        forwardInput = Mathf.Clamp(forwardInput, -1f, 1f);
        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);

        _car.HandleInput(forwardInput, horizontalInput, b);
    }

    /// <summary>
    /// State that attempts to fix a crash
    /// </summary>
    public void CourseCorrection()
    {
        if (_backTriggerCheck.active) _agentSpawnWeight = _spawnWeight;
        else if (_frontTriggerCheck.active) _agentSpawnWeight = -_spawnWeight;
        else _agentSpawnWeight = _defaultSpawnWeight;
        if (_agentSpawnWeight != _defaultSpawnWeight) _agent.speed = _weightedAgentAcc;
        else _agent.speed = _defaultAgentAcc;
        if (Vector3.Distance(transform.position, _agent.transform.position) > _distanceBetweenAgent)
        {
            _agent.isStopped = true;
        }
        else
        {
            _agent.isStopped = false;
        }
        if (Mathf.Abs(_agent.transform.position.y - transform.position.y) > _yWarp)
        {
            _agent.Warp(transform.position);
        }
    }

    public void RecallAgent()
    {
        Vector3 pos = transform.position;
        pos += transform.forward * _agentSpawnWeight;
        SetAgentTarget(pos);
    }

    public bool FindPath(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(target, path);
        if (path.status == NavMeshPathStatus.PathPartial || path.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        }
        return true;
    }

    public bool StalePath()
    {
        return _agent.isPathStale;
    }

    public void SetAgentTarget(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    private void IdleTiming()
    {
        if (_car.GetSpeed() < 3)
        {
            _idleTimer += Time.deltaTime;

            if (_idleTimer > _maxIdleTimer)
            {
                _stuck = true;
            }
        }
        else
        {
            _stuck = false;
            _idleTimer = 0f;
        }
    }

    public void OnDrawGizmos()
    {
        if (_agent)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(_agent.transform.position, .5f);
        }
    }
}