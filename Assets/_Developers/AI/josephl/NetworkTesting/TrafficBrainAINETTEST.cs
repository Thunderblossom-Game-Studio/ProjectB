using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficBrainAINETTEST : NetworkBehaviour
{

    [Header("Waypoint Targets")]
    [SyncVar] public Transform goal;
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
    [SyncVar][SerializeField] float CarSpeed;
    float DefaultSpeed;
    [SerializeField] float PanicCarSpeed;
    bool PanicForever;
    float SecondsToWait;

    [Header("Health")]
    public float Health;

    [Header("Donuts")]
    public bool ActivateDonut;
    public GameObject ObjectToDonut;
    public int SpinY;

    public override void OnStartServer()
    {
        base.OnStartServer();

        if (!base.IsServer)
            return;
        
        panic = ShowPanic;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        DefaultSpeed = CarSpeed;
    }

    void Update()
    {
        if (!base.IsServer)
            return;

        #region
        if (Vector3.Distance(transform.position, goal.transform.position) < 1)
        {
            goal.GetComponent<WaypointControlAINETTEST>().Lane(this);
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

        Vector3 sortedgoal = goal.transform.position;

        sortedgoal.z = transform.position.z;

        if (Vector3.Distance(transform.position, sortedgoal) <= .1f)
        {
            goal.GetComponent<WaypointControlAINETTEST>().Lane(this);
            agent.destination = goal.position;
        }
        
        Vector3 MovingForward = PointyTheSequel.transform.TransformDirection(Vector3.forward) * CarSpeed;
        RaycastHit AnotherHit;
        
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

        if (panic == true && CarmageddonMode == false)
        {
            StartCoroutine(PanicMode());
            panic = false;
        }

        if (CarmageddonMode == true)
        {
            IgnoreRaycasts = true;
            //CarSpeed = PanicCarSpeed;
            SetSpeed(PanicCarSpeed);
            PanicForever = true;
            StartCoroutine(PanicMode());
            CarmageddonMode = false;
        }

        if (PanicForever == true)
        {
            SecondsToWait = 0.2f;
        }
        else
        {
            SecondsToWait = 1;
        }



        if (Health <= 0)
        {
            panic = false;
            CarmageddonMode = false;
            PanicForever = false;
            Explode();
        }

        if (ActivateDonut == true)
        {
            Donuts();
        }


    }

    [ServerRpc (RequireOwnership = false)]
    private void SetSpeed(float speed)
    {
        Debug.Log("Speed changed");
        CarSpeed = speed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (CompareTag("Player"))
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



    void Donuts()
    {
        agent.isStopped = true;
        ObjectToDonut.transform.Rotate(new Vector3(0, SpinY, 0));

    }

    void Explode()
    {
        //insert explosion effects here
    }


}
