using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class TrafficBrain : MonoBehaviour
{

    [Header("Waypoint Targets")]
    #region References
    [Tooltip("Traffic Car's Next Target Waypoint/Movement")]
    public Transform goal;
    public Transform panicgoal;
    Transform savegoal;
    #endregion

    [Header("Panic States")]
    #region References
    [Tooltip("If This Is Ticked, The Traffic Car Will Start To Panic And Drive Frantically.")]
    public bool panic;
    public bool ShowPanic;
    [Tooltip("If This Is Ticked, The Traffic Car Will Panic Forever.")]
    public bool ExtendedPanic;
    [SerializeField] int DistanceForwardIncrease;
    public int PastPanicAxis;  
    public int PanicAxis;
    bool PanicForever;
    int ForLoopLength = 3;
    float KeepX;
    #endregion

    [Header("Raycast")]
    #region
    public GameObject LeftRayCast;
    public GameObject RightRayCast;
    UnityEngine.AI.NavMeshAgent agent;
    bool IgnoreRaycasts;
    [SerializeField] float RayCastInt;
    float SecondsToWait;
    #endregion

    [Header("Health")]
    #region
    [Tooltip("The Traffic Car's Current Health.")]
    public float Health;
    [Tooltip("The Maximum Health Of The Traffic Car.")]
    public float MaxHealth;
    #endregion

    [Header("SpinOut")]
    #region
    public bool ActivateSpinOut;
    public GameObject ObjectToSpinOut;
    public GameObject anchor;
    public int SpinY;
    public float Thrust = 20f;
    #endregion

    [Header("Wheels")]
    #region
    public GameObject FrontLeft;
    public GameObject FrontRight;
    public GameObject RearLeft;
    public GameObject RearRight;
    public float turnSpeed = 100f;
    #endregion

    [Header("Death")]
    #region
    public GameObject SpawnStation;
    public GameObject Itself;
    #endregion

    void Start()
    {
        panic = ShowPanic;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        #region Raycast Code
        if (Vector3.Distance(transform.position, goal.transform.position) < 1)
        {
            goal.GetComponent<WaypointControl>().Car = this.gameObject;
            goal.GetComponent<WaypointControl>().Lane();
            agent.destination = goal.position;
        }
        RaycastHit hit;
        Vector3 forward = LeftRayCast.transform.TransformDirection(Vector3.forward) * RayCastInt;
        if (Physics.Raycast(LeftRayCast.transform.position, forward, out hit, 5.0f))
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
        Vector3 MovingForward = RightRayCast.transform.TransformDirection(Vector3.forward) * RayCastInt;
        if (Physics.Raycast(RightRayCast.transform.position, MovingForward, out AnotherHit, 5.0f))
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

        if(panic == true && ExtendedPanic == false)
        {
            StartCoroutine(PanicMode());
            panic = false;
        }

        if(ExtendedPanic == true)
        {
            IgnoreRaycasts = true;
            PanicForever = true;
            StartCoroutine(PanicMode());
            ExtendedPanic= false;
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
            ExtendedPanic = false;
            PanicForever = false;
            ActivateSpinOut = true;
        }

        if (ActivateSpinOut == true)
        {
            SpinOut();
        }

        FrontLeft.transform.Rotate(Vector3.back, turnSpeed * Time.deltaTime);
        FrontRight.transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
        RearLeft.transform.Rotate(Vector3.back, turnSpeed * Time.deltaTime);
        RearRight.transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
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
        PanicAxis = 0;
        for (int i = 0; i < ForLoopLength; i++)
        {
            PastPanicAxis = PanicAxis;
            PanicAxis = Random.Range(0, 7);
            panicgoal.transform.position = transform.position;
            panicgoal.transform.localPosition = new Vector3((PanicAxis - PastPanicAxis), 0, 4);
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
    }

    void SpinOut()
    {
        agent.enabled = false;
        Destroy(anchor);
        LeftRayCast.SetActive(false);
        RightRayCast.SetActive(false);
        FrontLeft.SetActive(false);
        FrontRight.SetActive(false);
        ObjectToSpinOut.GetComponent<Rigidbody>().AddForce(transform.forward * Thrust);
        ObjectToSpinOut.transform.Rotate(new Vector3(0, SpinY, 0));
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(3);
        //Insert Explosion/Particle Effects Here
        SpawnStation.GetComponent<SpawnerControl>().count = SpawnStation.GetComponent<SpawnerControl>().count - 1;
        Destroy(Itself);
    }
}

//[CustomEditor(typeof(TrafficBrain))]
//public class TrafficStatEditor : Editor
//{
//    // The various categories the editor will display the variables in 
//    public enum DisplayCategory
//    {
//        Basic, Panic, Health
//    }

//    // The enum field that will determine what variables to display in the Inspector
//    public DisplayCategory categoryToDisplay;

//    // The function that makes the custom editor work
//    public override void OnInspectorGUI()
//    {
//        // Display the enum popup in the inspector
//        categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);

//        // Create a space to separate this enum popup from other variables 
//        EditorGUILayout.Space();

//        // Switch statement to handle what happens for each category
//        switch (categoryToDisplay)
//        {
//            case DisplayCategory.Basic:
//                DisplayBasicInfo();
//                break;

//            case DisplayCategory.Panic:
//                DisplayPanicInfo();
//                break;

//            case DisplayCategory.Health:
//                DisplayHealthInfo();
//                break;

//        }
//        serializedObject.ApplyModifiedProperties();
//    }

//    // When the categoryToDisplay enum is at "Basic"
//    void DisplayBasicInfo()
//    {
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("goal"));

//    }

//    // When the categoryToDisplay enum is at "Panic"
//    void DisplayPanicInfo()
//    {
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("panic"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("ExtendedPanic"));
//    }

//    // When the categoryToDisplay enum is at "Health"
//    void DisplayHealthInfo()
//    {

//        EditorGUILayout.PropertyField(serializedObject.FindProperty("Health"));
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxHealth"));
//    }
//}