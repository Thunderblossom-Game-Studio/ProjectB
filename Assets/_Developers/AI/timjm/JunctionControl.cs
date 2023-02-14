using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionControl : MonoBehaviour
{
    public GameObject EastToNorth;
    public GameObject EastToWest;
    public GameObject EastToSouth;

    public GameObject WestToNorth;
    public GameObject WestToEast;
    public GameObject WestToSouth;

    public GameObject NorthToEast;
    public GameObject NorthToWest;
    public GameObject NorthToSouth;

    public GameObject SouthToNorth;
    public GameObject SouthToWest;
    public GameObject SouthToEast;

    public bool Change;
    public int Sequence;

    // Update is called once per frame
    void Update()
    {
        if (Change == true)
        {
            StartCoroutine("Swap");
            Change = false;
        }
    }

    IEnumerator Swap()
    {
        Sequence = Random.Range(0, 7);
        if (Sequence == 0)
        {
            EastToNorth.GetComponent<WaypointControl>().Red = true;
            EastToWest.GetComponent<WaypointControl>().Red = false;
            EastToSouth.GetComponent<WaypointControl>().Red = false;
            WestToNorth.GetComponent<WaypointControl>().Red = false;
            WestToEast.GetComponent<WaypointControl>().Red = false;
            WestToSouth.GetComponent<WaypointControl>().Red = true;
            NorthToEast.GetComponent<WaypointControl>().Red = true;
            NorthToWest.GetComponent<WaypointControl>().Red = true;
            NorthToSouth.GetComponent<WaypointControl>().Red = true;
            SouthToNorth.GetComponent<WaypointControl>().Red = true;
            SouthToWest.GetComponent<WaypointControl>().Red = true;
            SouthToEast.GetComponent<WaypointControl>().Red = true;
        }
        if (Sequence == 1)
        {
            EastToNorth.GetComponent<WaypointControl>().Red = true;
            EastToWest.GetComponent<WaypointControl>().Red = true;
            EastToSouth.GetComponent<WaypointControl>().Red = true;
            WestToNorth.GetComponent<WaypointControl>().Red = true;
            WestToEast.GetComponent<WaypointControl>().Red = true;
            WestToSouth.GetComponent<WaypointControl>().Red = true;
            NorthToEast.GetComponent<WaypointControl>().Red = false;
            NorthToWest.GetComponent<WaypointControl>().Red = true;
            NorthToSouth.GetComponent<WaypointControl>().Red = false;
            SouthToNorth.GetComponent<WaypointControl>().Red = false;
            SouthToWest.GetComponent<WaypointControl>().Red = false;
            SouthToEast.GetComponent<WaypointControl>().Red = true;
        }
        if (Sequence == 2)
        {
            EastToNorth.GetComponent<WaypointControl>().Red = true;
            EastToWest.GetComponent<WaypointControl>().Red = true;
            EastToSouth.GetComponent<WaypointControl>().Red = true;
            WestToNorth.GetComponent<WaypointControl>().Red = false;
            WestToEast.GetComponent<WaypointControl>().Red = true;
            WestToSouth.GetComponent<WaypointControl>().Red = true;
            NorthToEast.GetComponent<WaypointControl>().Red = true;
            NorthToWest.GetComponent<WaypointControl>().Red = false;
            NorthToSouth.GetComponent<WaypointControl>().Red = true;
            SouthToNorth.GetComponent<WaypointControl>().Red = true;
            SouthToWest.GetComponent<WaypointControl>().Red = true;
            SouthToEast.GetComponent<WaypointControl>().Red = true;
        }
        if (Sequence == 3)
        {
            EastToNorth.GetComponent<WaypointControl>().Red = true;
            EastToWest.GetComponent<WaypointControl>().Red = true;
            EastToSouth.GetComponent<WaypointControl>().Red = false;
            WestToNorth.GetComponent<WaypointControl>().Red = true;
            WestToEast.GetComponent<WaypointControl>().Red = true;
            WestToSouth.GetComponent<WaypointControl>().Red = true;
            NorthToEast.GetComponent<WaypointControl>().Red = true;
            NorthToWest.GetComponent<WaypointControl>().Red = true;
            NorthToSouth.GetComponent<WaypointControl>().Red = true;
            SouthToNorth.GetComponent<WaypointControl>().Red = true;
            SouthToWest.GetComponent<WaypointControl>().Red = true;
            SouthToEast.GetComponent<WaypointControl>().Red = false;
        }
        if (Sequence == 4)
        {
            EastToNorth.GetComponent<WaypointControl>().Red = true;
            EastToWest.GetComponent<WaypointControl>().Red = true;
            EastToSouth.GetComponent<WaypointControl>().Red = true;
            WestToNorth.GetComponent<WaypointControl>().Red = true;
            WestToEast.GetComponent<WaypointControl>().Red = true;
            WestToSouth.GetComponent<WaypointControl>().Red = false;
            NorthToEast.GetComponent<WaypointControl>().Red = true;
            NorthToWest.GetComponent<WaypointControl>().Red = true;
            NorthToSouth.GetComponent<WaypointControl>().Red = true;
            SouthToNorth.GetComponent<WaypointControl>().Red = true;
            SouthToWest.GetComponent<WaypointControl>().Red = false;
            SouthToEast.GetComponent<WaypointControl>().Red = true;
        }
        if (Sequence == 5)
        {
            EastToNorth.GetComponent<WaypointControl>().Red = false;
            EastToWest.GetComponent<WaypointControl>().Red = true;
            EastToSouth.GetComponent<WaypointControl>().Red = true;
            WestToNorth.GetComponent<WaypointControl>().Red = true;
            WestToEast.GetComponent<WaypointControl>().Red = true;
            WestToSouth.GetComponent<WaypointControl>().Red = true;
            NorthToEast.GetComponent<WaypointControl>().Red = false;
            NorthToWest.GetComponent<WaypointControl>().Red = true;
            NorthToSouth.GetComponent<WaypointControl>().Red = true;
            SouthToNorth.GetComponent<WaypointControl>().Red = true;
            SouthToWest.GetComponent<WaypointControl>().Red = true;
            SouthToEast.GetComponent<WaypointControl>().Red = true;
        }
        yield return new WaitForSeconds(5);
        Change = true;
    }
}
