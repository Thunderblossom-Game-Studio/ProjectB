using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBrain : MonoBehaviour
{
    [Header("Waypoint Targets")]
    public Transform goal;
    public Transform panicgoal;
    public Transform savegoal;
    public float KeepX;
    public GameObject pointy;

    int ForLoopLength = 3;

    public GameObject PointyTheSequel;
    //Vector3 paniclocation;
    UnityEngine.AI.NavMeshAgent agent;

    [Header("Panic States")]
    public static bool panic;
    public bool ShowPanic;
    public bool CarmageddonMode;
    [SerializeField] int DistanceForwardIncrease = 2;
    //int DistanceForward = 0;
    public int RPast;
    public int R;


    
    private bool IgnoreRaycasts;

    [Header("Car Speed")]
    [SerializeField] float CarSpeed;
    float DefaultSpeed;
    [SerializeField] float PanicCarSpeed;
    bool PanicForever;
    float SecondsToWait;


    void Start()
    {
        panic = ShowPanic;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        DefaultSpeed = CarSpeed;
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
        Vector3 forward = pointy.transform.TransformDirection(Vector3.forward) * CarSpeed;
        if (Physics.Raycast(pointy.transform.position, forward, out hit, 5.0f))
        {
            if (hit.rigidbody != null && IgnoreRaycasts == false)
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
        Vector3 MovingForward = PointyTheSequel.transform.TransformDirection(Vector3.forward) * CarSpeed;
        if (Physics.Raycast(PointyTheSequel.transform.position, MovingForward, out AnotherHit, 5.0f))
        {
            if (AnotherHit.rigidbody != null && IgnoreRaycasts == false)
            {
                agent.isStopped = true;
            }
        }
        else
        {
            agent.isStopped = false;
        }
        #endregion

        if(panic == true && CarmageddonMode == false)
        {
            StartCoroutine(PanicMode());
            panic = false;
        }

        if(CarmageddonMode == true)
        {
            IgnoreRaycasts = true;
            CarSpeed = PanicCarSpeed;
            PanicForever = true;
            StartCoroutine(PanicMode());
            CarmageddonMode= false;
        }

        if (PanicForever == true)
        {
            SecondsToWait = 0.2f;
        }
        else
        {
            SecondsToWait = 1;
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
        agent.autoBraking = false;
        R = 0;
        for (int i = 0; i < ForLoopLength; i++)
        {
            RPast = R;
            R = Random.Range(0, 7);
            //panicgoal.position = new Vector3((KeepX + (Random.Range(0, 7))), transform.position.y, (transform.position.z + DistanceForward));
            panicgoal.transform.position = transform.position;
            panicgoal.transform.localPosition = new Vector3((R - RPast), 0, 4);
            goal = panicgoal;
            agent.isStopped = true;
            agent.ResetPath();
            agent.isStopped = false;
            agent.SetDestination(goal.position);

            yield return new WaitForSeconds(SecondsToWait);

            DistanceForwardIncrease += 5;

            if (PanicForever == true)
            {
                i = 0;
            }


        }

        DistanceForwardIncrease += 2;
        agent.isStopped = true;
        agent.ResetPath();
        agent.isStopped = false;
        agent.SetDestination(savegoal.position);
        goal = savegoal;
        agent.autoBraking = true;
    }
    


}
