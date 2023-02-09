using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AITestCar))]
public class AICarController : MonoBehaviour
{
    #region Test Scene Specific Definitions

    // Test States Definition
    public enum DemoAIState { Chase, Evade, Patrol }

    [Header("Test Scene Specific")]

    // Test States Declaration
    public DemoAIState currentState = DemoAIState.Chase;
        
    // Test Car Class Called
    protected AITestCar car;

    #endregion

    [Header("Pathfinding Settings")]

    [SerializeField] protected NavMeshAgent agent;

    protected bool newState = false;
    
    internal bool crashed = false;
    protected float crashReverseTime = 0;

    #region Modifiable Controller Functions
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        car = GetComponent<AITestCar>();
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

        car.Accelerate(-1);

        car.Turn(1);

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

        DemoAIState c = currentState;

        Evaluate();

        if (c != currentState && !(currentState == DemoAIState.Evade && (Vector3.Distance(transform.position, agent.destination) < 10))) newState = true;

        SwapState();
    }

    /// <summary>
    /// Makes the car move in the direction of the navmesh agent it's chasing at all times
    /// </summary>
    protected virtual void FollowAgent()
    {
        Vector3 dir = (agent.transform.position - transform.position).normalized;

        float direction = Vector3.Dot(dir, transform.forward);

        float distance = Vector3.Distance(transform.position, agent.transform.position);

        if (direction < -0.3f)
        {
            car.Accelerate(-1);
        }
        else if (direction > 0.3f)
        {
            car.CustomAccelerate(1,distance*2);
        }

        float direction2 = Vector3.Dot(dir, transform.right);

        if (direction2 < -0.3f)
        {
            car.Turn(-1);
        }
        else if (direction2 > 0.3f)
        {
            car.Turn(1);
        }
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