using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointControlAINETTEST : NetworkBehaviour
{
    public List<GameObject> Exits;

    [SyncVar] public GameObject Next;
    [SyncVar] public bool Red;
    [SyncVar] bool ActivatePanic;

    [ServerRpc (RequireOwnership = false)]
    public void Lane(TrafficBrainAINETTEST car)
    {
        // Skip if no exits
        if (Exits.Count == 0) return;

        // Select next exit
        Next = Exits[Random.Range(0, Exits.Count)];
        Debug.Log("Next exit: " + Next.gameObject.name);

        // If panic, panic, else move to next waypoint
        if (ActivatePanic == true)
        {
            TrafficBrainAINETTEST.panic = true;
            ActivatePanic = false;
        }
        else
        {
            car.goal = Next.transform;
            Debug.Log("Waypoint goal transform updated");
        }
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
