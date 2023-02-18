using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBrain : MonoBehaviour
{
    public Transform goal;
    public Transform panicgoal;
    public Transform savegoal;
    public float KeepX;
    public GameObject pointy;

    int ForLoopLength = 3;

    public GameObject PointyTheSequel;
    Vector3 paniclocation;
    UnityEngine.AI.NavMeshAgent agent;
    public bool panic;
    [SerializeField] int DistanceForward = 5;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
    }

    // Update is called once per frame
    void Update()
    {
        #region
        if (Vector3.Distance(transform.position, goal.transform.position) < 1)
        {
            goal.GetComponent<WaypointControl>().Car = this.gameObject;
            goal.GetComponent<WaypointControl>().Lane();
            agent.destination = goal.position;
        }
        RaycastHit hit;
        Vector3 forward = pointy.transform.TransformDirection(Vector3.forward) * 2;
        if (Physics.Raycast(pointy.transform.position, forward, out hit, 5.0f))
        {
            if (hit.rigidbody != null)
            {
                agent.isStopped = true;
            }
        }
        else
        {
            agent.isStopped = false;
        }

        if (Vector3.Distance(transform.position, goal.transform.position) < 1)
        {
            goal.GetComponent<WaypointControl>().Car = this.gameObject;
            goal.GetComponent<WaypointControl>().Lane();
            agent.destination = goal.position;
        }
        RaycastHit AnotherHit;
        Vector3 MovingForward = PointyTheSequel.transform.TransformDirection(Vector3.forward) * 2;
        if (Physics.Raycast(PointyTheSequel.transform.position, MovingForward, out AnotherHit, 5.0f))
        {
            if (AnotherHit.rigidbody != null)
            {
                agent.isStopped = true;
            }
        }
        else
        {
            agent.isStopped = false;
        }
        #endregion

        if(panic == true)
        {
            StartCoroutine(PanicMode());
            panic = false;
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if(CompareTag("Player"))
        {
           StartCoroutine(PanicMode());
        }
    }

    
    IEnumerator PanicMode()
    {
        //point based off rng local
        //point based off current pos + rng values
        savegoal = goal;
        KeepX = transform.position.x;

        for (int i = 0; i < ForLoopLength; i++)
        {
        panicgoal.position = new Vector3((KeepX + (Random.Range(0, 7))), transform.position.y, (transform.position.z + DistanceForward));
        goal = panicgoal;
        agent.isStopped = true;
        agent.ResetPath();
        agent.isStopped = false;
        agent.SetDestination(goal.position);
        yield return new WaitForSeconds(3);

        }
        


    }
    


}
