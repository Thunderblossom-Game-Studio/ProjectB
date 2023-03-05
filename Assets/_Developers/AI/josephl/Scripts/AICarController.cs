using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIVehicleController))]
public class AICarController : MonoBehaviour
{
    protected AIVehicleController car;

    [Header("Pathfinding Settings")]

    [SerializeField] protected NavMeshAgent agent;
    [Range(0f, 1f)] [SerializeField] protected float forwardmultiplier;
    [Range(0f, 5f)] [SerializeField] protected float turnmultiplier;
    [Range(0, 100)] [SerializeField] protected float brakeSensitivity = 50;
    [Range(0, 180)] [SerializeField] protected int angle;
    [SerializeField] protected float stopDistance = 10;

    protected bool newState = false;
    
    internal bool crashed = false;
    protected float crashReverseTime = 0;

    #region Modifiable Controller Functions
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        car = GetComponent<AIVehicleController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!agent)
        {
            Debug.LogWarning("No NavMesh Agent Assigned");

            return;
        }

        Act();
    }

    /// <summary>
    /// Runs a series of checks to choose what state is appropriate based on the current data provided
    /// </summary>
    protected virtual void Evaluate()
    {

    }

    /// <summary>
    /// Runs the state selected in Evaluate()
    /// </summary>
    protected virtual void SwapState()
    {

    }

    /// <summary>
    /// State that attempts to fix a crash
    /// </summary>
    protected virtual void CourseCorrect()
    {
        crashReverseTime += Time.deltaTime;

        if (crashReverseTime > .5f)
        {
            crashed = false;

            //agent.transform.position = ha.shootpoint.position;

            crashReverseTime = 0;
        }

        
        car.HandleInput(-1, 1, false);

        newState = false;
    }

    #endregion

    #region Core Controller Functions

    /// <summary>
    /// Runs the AI controllers frame functionality from path finding to state evaluation + execution
    /// </summary>
    protected virtual void Act()
    {
        FollowAgent();

        Evaluate();


        SwapState();
    }

    /// <summary>
    /// Makes the car move in the direction of the navmesh agent it's chasing at all times
    /// </summary>
    protected virtual void FollowAgent()
    {
        Vector3 dir = (agent.transform.position - transform.position).normalized;

        //float direction = Vector3.Dot(dir, transform.forward);

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

        // brakesens
        // stoppingdis

        //Debug.Log(car.GetSpeed());

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

        if (b) Debug.Log("braking");

        forwardInput = Mathf.Clamp(forwardInput, -1f, 1f);
        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);

        car.HandleInput(forwardInput, horizontalInput, b);
    }

    #endregion

    protected virtual void OnDrawGizmos()
    {
        if (agent)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(agent.transform.position, .5f);
        }
    }
}