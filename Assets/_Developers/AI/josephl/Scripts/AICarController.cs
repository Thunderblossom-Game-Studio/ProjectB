
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICarController : MonoBehaviour
{
   // private VehicleParent _car;

    [Header("Pathfinding Settings")]

    [SerializeField] private NavMeshAgent _agent;
    [Range(0f, 1f)] [SerializeField] private float _forwardmultiplier;
    [Range(0f, 5f)] [SerializeField] private float _turnmultiplier;
    [Range(0, 100)] [SerializeField] private float _brakeSensitivity = 50;
    [Range(0, 100)] [SerializeField] private float _speedSensitivity = 50;
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

    [SerializeField] private float _maxIdleTimer = 20;
    private float _idleTimer = 0;
    private bool _stuck = false;

    private float _distanceMultiplier = 1.6f;

   // private BasicInput.MoveData _md;

    // Start is called before the first frame update
    private void Start()
    {
       // _car = GetComponent<VehicleParent>();

        if (_agent)
            _agent.Warp(transform.position);
    }

    private void Update()
    {
        if (!_agent)
        {
            Debug.LogWarning("No NavMesh Agent Assigned");

            return;
        }

        FollowAgent();
        CourseCorrection();
        IdleTiming();
    }

    private void FixedUpdate()
    {
        if (!_agent)
        {
            Debug.LogWarning("No NavMesh Agent Assigned");

            return;
        }

    }

    public AIPlayerHandler.CurrentState Evaluate(AIPlayerHandler.CurrentState state)
    {
        return state;
    }

    /// <summary>
    /// Makes the car move in the direction of the navmesh agent it's chasing at all times
    /// </summary>
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
            forwardInput = 1;
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

        if (forwardInput != 0) 
            horizontalInput *= forwardInput;


        //if ((_car.localVelocity.magnitude > _speedSensitivity)
        //    ||
        //    ((Vector3.Distance(transform.position, _agent.transform.position) < _stopDistance
        //    &&
        //    _car.localVelocity.magnitude > _brakeSensitivity)))
        //{
        //    forwardInput = -.25f;
        //}
        //// braking
        //int brake = 0;

        //if (Mathf.Abs(horizontalInput) >= 0.6f && _car.localVelocity.magnitude > _brakeSensitivity)
        //{
        //    brake = 1;
        //}

        //forwardInput = Mathf.Clamp(forwardInput, -.75f, .75f);
        //horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);

        //_md.AccelInput = forwardInput;
        //_md.SteerInput = horizontalInput;
        //_md.EbrakeInput = brake;
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
        Vector3 agentYRemoved = _agent.transform.position;
        agentYRemoved.y = transform.position.y;
        if (Vector3.Distance(transform.position, agentYRemoved) > _distanceBetweenAgent)
        {
            _agent.isStopped = true;
        }
        else
        {
            _agent.isStopped = false;
        }
        if (_frontTriggerCheck.active || _backTriggerCheck.active)
        {
            _agent.isStopped = false;
            RecallAgent();
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
        //if (_car.localVelocity.magnitude < 3)
        //{
        //    _idleTimer += Time.deltaTime;

        //    if (_idleTimer > _maxIdleTimer)
        //    {
        //        _stuck = true;
        //        transform.position = _agent.transform.position;
        //        transform.rotation = _agent.transform.rotation;
        //    }
        //}
        //else
        //{
        //    _stuck = false;
        //    _idleTimer = 0f;
        //}
    }

    public void OnDrawGizmos()
    {
        if (_agent)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(_agent.transform.position, .5f);
        }
    }

    //public void GetAIMoveData(out BasicInput.MoveData md)
    //{
    //    md = default;

    //    md = _md;
    //}
}