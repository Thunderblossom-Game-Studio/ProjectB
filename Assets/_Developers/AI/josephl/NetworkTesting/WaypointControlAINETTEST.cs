using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointControlAINETTEST : MonoBehaviour
{
    public List<GameObject> Exits;

    public int NextMarker;
    public GameObject Next;
    public bool Red;
    public GameObject Car;
    public bool ActivatePanic;

    public void Lane()
    {
        if (Exits.Count == 0) return;

        NextMarker = Random.Range(0, Exits.Count);

        Next = Exits[NextMarker];
        if (Red == true)
        {
            //StartCoroutine("Hold");
        }

        else if (ActivatePanic == true)
        {
            TrafficBrainAINETTEST.panic = true;
            ActivatePanic = false;
        }

        else
        {
            Car.GetComponent<TrafficBrainAINETTEST>().goal = Next.transform;
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

    IEnumerator Hold()
    {
        while (!Red)
        {
            yield return null;
        }
        Car.GetComponent<TrafficBrainAINETTEST>().goal = Next.transform;
    }
}