using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    [Header("To Spawn")]
    public GameObject Traffic;
    [Tooltip("Reference to prefab to spawn")]
    [Header("References")]
    public GameObject RoadConnect;
    public GameObject Grave;
    [Tooltip("Waypoint for the traffic to go to")]
    public GameObject Director;
    [Tooltip("The MasterWaypoint for the car")]
    public int PointIndex;
    [Tooltip("The Indexof the waypoint for the car")]
    [Header("Control Spawn")]
    public int limit;
    [Tooltip("Limit for the spawner to spawn")]

    public int count;
    [HideInInspector]


    bool ready = true;
    GameObject SpawnerReference;
    GameObject Clone;

    private void Awake()
    {
        SpawnerReference = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if ((ready == true)&&(count < limit))
        {   
            StartCoroutine(Spawn());
            ready = false;
        }
    }

    IEnumerator Spawn()
    {
        Clone = Instantiate(Traffic, transform.position, Quaternion.identity);       
        Clone.GetComponent<TrafficBrain>().SpawnStation = SpawnerReference;
        Clone.GetComponent<TrafficBrain>().goal = RoadConnect.transform;       
        Clone.GetComponent<TrafficBrain>().Index = PointIndex;
        Clone.GetComponent<TrafficBrain>().Director = Director;

        count += 1;
        yield return new WaitForSeconds(3);
        ready = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 forward = transform.position + transform.forward * 2;
        DrawArrow(gameObject.transform.position, forward, 45, 1,1);
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.1f, 1));
    }
    private void DrawArrow(Vector3 a, Vector3 b, float arrowheadAngle, float arrowheadDistance, float arrowheadLength)
    {
        // Get the Direction of the Vector
        Vector3 dir = b - a;

        // Get the Position of the Arrowhead along the length of the line.
        Vector3 arrowPos = a + (dir * arrowheadDistance);

        // Get the Arrowhead Lines using the direction from earlier multiplied by a vector representing half of the full angle of the arrowhead (y)
        // and -1 for going backwards instead of forwards (z), which is then multiplied by the desired length of the arrowhead lines coming from the point.

        Vector3 up = Quaternion.LookRotation(dir) * new Vector3(0f, Mathf.Sin(arrowheadAngle * Mathf.Deg2Rad), -1f) * arrowheadLength;
        Vector3 down = Quaternion.LookRotation(dir) * new Vector3(0f, -Mathf.Sin(arrowheadAngle * Mathf.Deg2Rad), -1f) * arrowheadLength;

        // Draw the line from A to B
        Gizmos.DrawLine(a, b);

        // Draw the rays representing the arrowhead.
        Gizmos.DrawRay(arrowPos, up);
        Gizmos.DrawRay(arrowPos, down);
    }



}
