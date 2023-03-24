using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class TrafficBrain : MonoBehaviour
{

    [Header("Waypoint Targets")]
    #region References
    [Tooltip("Traffic Car's Next Target Waypoint/Movement")]
    public Transform goal;
    public Transform panicgoal;
    Transform savegoal;
    public GameObject Director;
    public int Index;
    #endregion

    [Header("Panic States")]
    #region References
    [Tooltip("If This Is Ticked, The Traffic Car Will Start To Panic And Drive Frantically.")]
    public bool panic;
    [Tooltip("If This Is Ticked, extends the duration drastically.")]
    public bool ExtendedPanic;
    [SerializeField] int DistanceForwardIncrease;
    public int PastPanicAxis;  
    public int PanicAxis;
    [Tooltip("If This Is Ticked, The Traffic Car Will Panic Forever.")]
    public bool PanicForever;
    [Tooltip("Controls reaction strength")]
    public int ReactionLimit;
    [Tooltip("Controls reaction length")]
    public int ForLoopLength = 3;
    float KeepX;
    #endregion

    [Header("Raycast")]
    #region
    public GameObject RayCast;
    public UnityEngine.AI.NavMeshAgent agent;
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
    [Tooltip("The spin applied during spin out")]
    public int SpinY;
    [Tooltip("The thrust applied during spin out, must be above 100 to see effect")]
    public float Thrust = 120f;
    public bool ActivateSpinOut;
    public GameObject ObjectToSpinOut;
    #endregion

    [Header("Wheels")]
    #region
    [Tooltip("How fast the wheels spin")]
    public float turnSpeed = 100f;
    public GameObject FrontLeft;
    public GameObject FrontRight;
    public GameObject RearLeft;
    public GameObject RearRight;
    #endregion

    [Header("Death")]
    #region
    [Tooltip("Spawner this instance spawned from")]
    public GameObject SpawnStation;
    public bool DeathCleared;
    public int HowLongTillDeath = 3;
    #endregion

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        Health = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, goal.transform.position) < 1)
        {
            Director.GetComponent<WaypointDirector>().Car = this.gameObject;
            Director.GetComponent<WaypointDirector>().CarIndex = Index;
            Director.GetComponent<WaypointDirector>().CarBody = ObjectToSpinOut;
            Director.GetComponent<WaypointDirector>().Lane();
            agent.destination = goal.position;
        }

        //#region Raycast Code
        //RaycastHit hit;
        //Vector3 forward = RayCast.transform.TransformDirection(Vector3.forward) * RayCastInt; 
        //if (Physics.Raycast(RayCast.transform.position, forward, out hit, 5.0f))
        //{
        //    if (hit.rigidbody != null && IgnoreRaycasts == false)
        //    {
        //        agent.isStopped = true;
        //    }
        //}
        //else
        //{
        //    agent.isStopped = false;
        //}
        //#endregion

        #region Panic Code
        if (panic == true && ExtendedPanic == false)
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
        #endregion

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


        if (agent.destination != goal.position)
        {
            ReattachDestination();
        }

       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(CompareTag("Player"))
        {
           StartCoroutine(PanicMode());
        }

        if(CompareTag("Train"))
        {
            InstantExplosion();
        }
    }

    public void ActivatePanic()
    {
        StartCoroutine(PanicMode());
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
            PanicAxis = Random.Range(0, ReactionLimit);
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

    public void SpinOut()
    {
        agent.enabled = false;
        ObjectToSpinOut.GetComponent<Rigidbody>().AddForce(transform.forward * Thrust);
        ObjectToSpinOut.transform.Rotate(new Vector3(0, SpinY, 0));
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(HowLongTillDeath);
        //Insert Explosion/Particle Effects Here
        SpawnStation.GetComponent<SpawnerControl>().count = SpawnStation.GetComponent<SpawnerControl>().count - 1;
        this.gameObject.transform.position = SpawnStation.GetComponent<SpawnerControl>().Grave.transform.position;
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }

    void InstantExplosion()
    {
        SpawnStation.GetComponent<SpawnerControl>().count = SpawnStation.GetComponent<SpawnerControl>().count - 1;
        Destroy(this.gameObject);
    }


    void ReattachDestination()
    {
        agent.destination = goal.position;
    }

}

#if UNITY_EDITOR

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
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Director"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Index"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SpawnStation"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SpinY"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Thrust"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ActivateSpinOut"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("turnSpeed"));
    }

    // When the categoryToDisplay enum is at "Panic"
    void DisplayPanicInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("panic"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ExtendedPanic"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("PanicForever"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ReactionLimit"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("ForLoopLength"));
    }

    // When the categoryToDisplay enum is at "Health"
    void DisplayHealthInfo()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Health"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxHealth"));
    }
}

#endif