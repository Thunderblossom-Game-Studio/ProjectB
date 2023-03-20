using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointDirector : MonoBehaviour
{
    [System.Serializable]
    public class Waypoint
    {
        public GameObject Point;
        public List<GameObject> Exits;
    }
    public Waypoint[] WaypointArray;

    [HideInInspector]
    public int NextMarker;
    [HideInInspector]
    public GameObject Next;
    [HideInInspector]
    public GameObject Car;
    [HideInInspector]
    public int CarIndex;

    public void Lane()
    {
        if (WaypointArray[CarIndex].Exits.Count == 0) return;

        NextMarker = Random.Range(0, WaypointArray[CarIndex].Exits.Count);

        Next = WaypointArray[CarIndex].Exits[NextMarker];
        
        Car.GetComponent<TrafficBrain>().goal = Next.transform;
        for (int i = 0; i < WaypointArray.Length; i++)    
        {    
           if(WaypointArray[i].Point == Next)
           {     
              Car.GetComponent<TrafficBrain>().Index = i;  
              break;
           }          
        }        
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < WaypointArray.Length; i++)
        {
            Gizmos.DrawSphere(WaypointArray[i].Point.transform.position, .5f);

            foreach (GameObject exit in WaypointArray[i].Exits)
            {
                if (exit != null)
                {
                    // Draws a blue line from this transform to the target
                    Gizmos.DrawLine(WaypointArray[i].Point.transform.position, exit.transform.position);
                }
            }
        }
    }
}
