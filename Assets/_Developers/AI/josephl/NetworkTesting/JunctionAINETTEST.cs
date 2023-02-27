using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionAINETTEST : MonoBehaviour
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
            EastToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToWest.GetComponent<WaypointControlAINETTEST>().Red = false;
            EastToSouth.GetComponent<WaypointControlAINETTEST>().Red = false;
            WestToNorth.GetComponent<WaypointControlAINETTEST>().Red = false;
            WestToEast.GetComponent<WaypointControlAINETTEST>().Red = false;
            WestToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
        }
        if (Sequence == 1)
        {
            EastToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToEast.GetComponent<WaypointControlAINETTEST>().Red = false;
            NorthToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToSouth.GetComponent<WaypointControlAINETTEST>().Red = false;
            SouthToNorth.GetComponent<WaypointControlAINETTEST>().Red = false;
            SouthToWest.GetComponent<WaypointControlAINETTEST>().Red = false;
            SouthToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
        }
        if (Sequence == 2)
        {
            EastToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToNorth.GetComponent<WaypointControlAINETTEST>().Red = false;
            WestToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToWest.GetComponent<WaypointControlAINETTEST>().Red = false;
            NorthToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
        }
        if (Sequence == 3)
        {
            EastToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToSouth.GetComponent<WaypointControlAINETTEST>().Red = false;
            WestToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToEast.GetComponent<WaypointControlAINETTEST>().Red = false;
        }
        if (Sequence == 4)
        {
            EastToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToSouth.GetComponent<WaypointControlAINETTEST>().Red = false;
            NorthToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToWest.GetComponent<WaypointControlAINETTEST>().Red = false;
            SouthToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
        }
        if (Sequence == 5)
        {
            EastToNorth.GetComponent<WaypointControlAINETTEST>().Red = false;
            EastToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            EastToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
            WestToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToEast.GetComponent<WaypointControlAINETTEST>().Red = false;
            NorthToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            NorthToSouth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToNorth.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToWest.GetComponent<WaypointControlAINETTEST>().Red = true;
            SouthToEast.GetComponent<WaypointControlAINETTEST>().Red = true;
        }
        yield return new WaitForSeconds(5);
        Change = true;
    }
}
