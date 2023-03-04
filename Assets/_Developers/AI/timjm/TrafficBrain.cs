using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public bool panic;
    public bool ShowPanic;
    public bool CarmageddonMode;
    [SerializeField] int DistanceForwardIncrease = 2;
    //int DistanceForward = 0;
    public int RPast;
    public int R;


    
    bool IgnoreRaycasts;

    [Header("Car Speed")]
    [SerializeField] float CarSpeed;
    float DefaultSpeed;
    [SerializeField] float PanicCarSpeed;
    bool PanicForever;
    float SecondsToWait;

    [Header("Health")]
    public float Health;
    public float MaxHealth;

    [Header("Disable On Death")]

    //UPDATE THESE TO FIT NEW CAR MODEL:
    public BoxCollider TrunkCollider;
    public MeshRenderer TrunkRenderer;
    public MeshRenderer CubeRenderer;
    public MeshRenderer CylinderRenderer;
    public MeshRenderer AITrafficRenderer;
    public CapsuleCollider AITrafficCollider;
    public MeshRenderer PointerRenderer;
    public MeshRenderer Pointer1Renderer;
    public MeshRenderer Cylinder1Renderer;

    [Header("Respawn")]
    public GameObject RespawnPoint;
    public GameObject ThisGameObject;

    [Header("Donuts")]
    public bool ActivateDonut;
    public GameObject ObjectToDonut;
    public int SpinY;

    void Start()
    {
        panic = ShowPanic;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        DefaultSpeed = CarSpeed;
        Health = MaxHealth;
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


    
   void Donuts()
    {
        agent.isStopped = true;
        ObjectToDonut.transform.Rotate(new Vector3(0, SpinY, 0));
  
    }
    
    void Explode()
    {

        //insert explosion effects here
        TrunkCollider.enabled = false;
        TrunkRenderer.enabled = false;
        CubeRenderer.enabled = false;
        CylinderRenderer.enabled = false;
        AITrafficRenderer.enabled = false;
        AITrafficCollider.enabled = false;
        PointerRenderer.enabled = false;
        Cylinder1Renderer.enabled = false;
        StartCoroutine(Respawn());

}

    Vector3 RespawnPointVector;
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5);
       
        //transform.position = RespawnPoint.transform.position;
        RespawnPointVector = RespawnPoint.transform.position;

        transform.position = RespawnPointVector;
        ThisGameObject.transform.position = RespawnPointVector;


        Health = MaxHealth;
        goal = RespawnPoint.transform;
        agent.SetDestination(goal.position);
        TrunkCollider.enabled = true;
        TrunkRenderer.enabled = true;
        CubeRenderer.enabled = true;
        CylinderRenderer.enabled = true;
        AITrafficRenderer.enabled = true;
        AITrafficCollider.enabled = true;
        PointerRenderer.enabled = true;
        Cylinder1Renderer.enabled = true;

    }

}


[CustomEditor(typeof(TrafficBrain))]
public class TrafficStatEditor : Editor
{
    // The various categories the editor will display the variables in 
    public enum DisplayCategory
    {
        Basic, Panic, Health
    }

    // The enum field that will determine what variables to display in the Inspector
    public DisplayCategory categoryToDisplay;

    // The function that makes the custom editor work
    public override void OnInspectorGUI()
    {
        // Display the enum popup in the inspector
        categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);

        // Create a space to separate this enum popup from other variables 
        EditorGUILayout.Space();

        // Switch statement to handle what happens for each category
        switch (categoryToDisplay)
        {
            case DisplayCategory.Basic:
                DisplayBasicInfo();
                break;

            case DisplayCategory.Panic:
                DisplayPanicInfo();
                break;

            case DisplayCategory.Health:
                DisplayHealthInfo();
                break;

        }
        serializedObject.ApplyModifiedProperties();
    }

    // When the categoryToDisplay enum is at "Basic"
    void DisplayBasicInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("goal"));
    }

    // When the categoryToDisplay enum is at "Combat"
    void DisplayPanicInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("panic"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("CarmageddonMode"));
    }

    // When the categoryToDisplay enum is at "Magic"
    void DisplayHealthInfo()
    {

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Health"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxHealth"));
    }
}