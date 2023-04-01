
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointControlAINETTEST : MonoBehaviour
{
    public List<GameObject> Exits;

    public GameObject Next;

    public void ChangeToNextGoal(TrafficBrainAINETTEST car)
    {
        //Debug.Log(Exits.Count + " exits found");
        // Skip if no exits
        if (Exits.Count == 0) return;

        // Select next exit
        Next = Exits[Random.Range(0, Exits.Count)];
        //Debug.Log("Next exit: " + Next.name);

        car.goal = Next.transform;
        //Debug.Log("Waypoint goal transform updated");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(transform.position, .5f);

        foreach (GameObject exit in Exits)
        {
            if (exit != null)
            {
                // Draws a blue line from this transform to the target
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, exit.transform.position);
            }
        }
    }
}
