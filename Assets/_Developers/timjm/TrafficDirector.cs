using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficDirector : MonoBehaviour
{
    public List<GameObject> Exits;

    public int NextMarker;
    public GameObject Next;

    public GameObject Car;

    public bool trafficlight = false;

    public bool red = false;

    public float lightMaxTimer = 3f;
    private float lightTimer = 0f;

    private void Update()
    {
        if (trafficlight)
        {
            //if (red)
            //{
            //    lightTimer -= Time.deltaTime;

            //    if (lightTimer <= 0)
            //    {
            //        red = false;
            //    }
            //}
            //else
            //{
            //    lightTimer += Time.deltaTime;

            //    if (lightTimer > lightMaxTimer)
            //    {
            //        red = true;
            //    }
            //}
        }
        else
        {
            red = false;
        }
    }

    public void Lane()
    {
        if (Exits.Count == 0) return;

        NextMarker = Random.Range(0, Exits.Count);

        Next = Exits[NextMarker];

        Car.GetComponent<TrafficTrigger>().Target = Next;
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
