using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionControl : MonoBehaviour
{
    [Header("2 Way Junction Waypoints")]
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

    [Header("4 Way Junction Waypoints")]
    public GameObject EastLeftToWest;
    public GameObject EastLeftToNorth;
    public GameObject EastLeftToSouth;

    public GameObject EastRightToWest;
    public GameObject EastRightToNorth;
    public GameObject EastRightToSouth;

    public GameObject WestLeftToNorth;
    public GameObject WestLeftToEast;
    public GameObject WestLeftToSouth;

    public GameObject WestRightToNorth;
    public GameObject WestRightToEast;
    public GameObject WestRightToSouth;

    public GameObject NorthLeftToEast;
    public GameObject NorthLeftToSouth;
    public GameObject NorthLeftToWest;


    public GameObject NorthRightToEast;
    public GameObject NorthRightToSouth;
    public GameObject NorthRightToWest;


    public GameObject SouthLeftToWest;
    public GameObject SouthLeftToNorth;
    public GameObject SouthLeftToEast;

    public GameObject SouthRightToWest;
    public GameObject SouthRightToNorth;
    public GameObject SouthRightToEast;

    [Header("Controls")]
    public bool Change;
    public bool FourWayJunction;
    public int Sequence;

    [Header("Traffic Light Bool Variables")]
     bool EastToNorthLight;
     bool EastToWestLight;
     bool EastToSouthLight;   
     bool WestToNorthLight;
     bool WestToEastLight;
     bool WestToSouthLight;
     bool NorthToEastLight;
     bool NorthToWestLight;
     bool NorthToSouthLight;
     bool SouthToNorthLight;
     bool SouthToWestLight;
     bool SouthToEastLight;



    bool EastLeftLightToWest;
    bool EastLeftLightToNorth;
    bool EastLeftLightToSouth;

    bool EastRightLightToWest;
    bool EastRightLightToNorth;
    bool EastRightLightToSouth;

    bool WestLeftLightToNorth;
    bool WestLeftLightToEast;
    bool WestLeftLightToSouth;

    bool WestRightLightToNorth;
    bool WestRightLightToEast;
    bool WestRightLightToSouth;


    bool NorthLeftLightToEast;
    bool NorthLeftLightToSouth;
    bool NorthLeftLightToWest;


    bool NorthRightLightToEast;
    bool NorthRightLightToSouth;
    bool NorthRightLightToWest;

    bool SouthLeftLightToWest;
    bool SouthLeftLightToNorth;
    bool SouthLeftLightToEast;
    
    bool SouthRightLightToWest;
    bool SouthRightLightToNorth;
    bool SouthRightLightToEast;


    // Update is called once per frame

    void Update()
    {
        if (Change == true)
        {
            if (FourWayJunction == false)
            {
                StartCoroutine("Swap");
                Change = false;
            }

            if (FourWayJunction == true)
            {
                StartCoroutine("FourWaySwap");
                Change = false;
            }

        }

        EastToNorth.GetComponent<WaypointControl>().Red = EastToNorthLight;
        EastToWest.GetComponent<WaypointControl>().Red = EastToWestLight;
        EastToSouth.GetComponent<WaypointControl>().Red = EastToSouthLight;
        WestToNorth.GetComponent<WaypointControl>().Red = WestToNorthLight;
        WestToEast.GetComponent<WaypointControl>().Red = WestToEastLight;
        WestToSouth.GetComponent<WaypointControl>().Red = WestToSouthLight;
        NorthToEast.GetComponent<WaypointControl>().Red = NorthToEastLight;
        NorthToWest.GetComponent<WaypointControl>().Red = NorthToWestLight;
        NorthToSouth.GetComponent<WaypointControl>().Red = NorthToSouthLight;
        SouthToNorth.GetComponent<WaypointControl>().Red = SouthToNorthLight;
        SouthToWest.GetComponent<WaypointControl>().Red = SouthToWestLight;
        SouthToEast.GetComponent<WaypointControl>().Red = SouthToEastLight;



     EastLeftToWest.GetComponent<WaypointControl>().Red = EastLeftLightToWest;
     EastLeftToNorth.GetComponent<WaypointControl>().Red = EastLeftLightToNorth;
     EastLeftToSouth.GetComponent<WaypointControl>().Red  = EastLeftLightToSouth;

     EastRightToWest.GetComponent<WaypointControl>().Red  = EastRightLightToWest;
     EastRightToNorth.GetComponent<WaypointControl>().Red  = EastRightLightToNorth;
     EastRightToSouth.GetComponent<WaypointControl>().Red = EastRightLightToSouth;

     WestLeftToNorth.GetComponent<WaypointControl>().Red = WestLeftLightToNorth;
     WestLeftToEast.GetComponent<WaypointControl>().Red  = WestLeftLightToEast;
     WestLeftToSouth.GetComponent<WaypointControl>().Red = WestLeftLightToSouth;

     WestRightToNorth.GetComponent<WaypointControl>().Red = WestRightLightToNorth;
     WestRightToEast.GetComponent<WaypointControl>().Red = WestRightLightToEast;
     WestRightToSouth.GetComponent<WaypointControl>().Red = WestRightLightToSouth;

     NorthLeftToEast.GetComponent<WaypointControl>().Red = NorthLeftLightToEast;
     NorthLeftToSouth.GetComponent<WaypointControl>().Red = NorthLeftLightToSouth;
     NorthLeftToWest.GetComponent<WaypointControl>().Red = NorthLeftLightToWest;


     NorthRightToEast.GetComponent<WaypointControl>().Red  = NorthRightLightToEast;
     NorthRightToSouth.GetComponent<WaypointControl>().Red = NorthRightLightToSouth;
     NorthRightToWest.GetComponent<WaypointControl>().Red = NorthRightLightToWest;


     SouthLeftToWest.GetComponent<WaypointControl>().Red = SouthLeftLightToWest;
     SouthLeftToNorth.GetComponent<WaypointControl>().Red = SouthLeftLightToNorth;
     SouthLeftToEast.GetComponent<WaypointControl>().Red = SouthLeftLightToEast;

     SouthRightToWest.GetComponent<WaypointControl>().Red = SouthRightLightToWest;
     SouthRightToNorth.GetComponent<WaypointControl>().Red  = SouthRightLightToNorth;
     SouthRightToEast.GetComponent<WaypointControl>().Red = SouthRightLightToEast;


    }

    IEnumerator Swap()
    {
        Sequence = Random.Range(0, 6);  
        if (Sequence == 0)
        {
            EastToNorthLight = true;
            EastToWestLight = false;
            EastToSouthLight = false;
            WestToNorthLight = false;
            WestToEastLight = false;
            WestToSouthLight = true;
            NorthToEastLight = true;
            NorthToWestLight = true;
            NorthToSouthLight = true;
            SouthToNorthLight = true;
            SouthToWestLight = true;
            SouthToEastLight = true;
        }
        if (Sequence == 1)
        {
            EastToNorthLight = true;
            EastToWestLight = true;
            EastToSouthLight = true;
            WestToNorthLight = true;
            WestToEastLight = true;
            WestToSouthLight = true;
            NorthToEastLight = false;
            NorthToWestLight = true;
            NorthToSouthLight = false;
            SouthToNorthLight = false;
            SouthToWestLight = false;
            SouthToEastLight = true;
        }
        if (Sequence == 2)
        {
            EastToNorthLight = true;
            EastToWestLight = true;
            EastToSouthLight = true;
            WestToNorthLight = false;
            WestToEastLight = true;
            WestToSouthLight = true;
            NorthToEastLight = true;
            NorthToWestLight = false;
            NorthToSouthLight = true;
            SouthToNorthLight = true;
            SouthToWestLight = true;
            SouthToEastLight = true;
        }
        if (Sequence == 3)
        {
            EastToNorthLight = true;
            EastToWestLight = true;
            EastToSouthLight = false;
            WestToNorthLight = true;
            WestToEastLight = true;
            WestToSouthLight = true;
            NorthToEastLight = true;
            NorthToWestLight = true;
            NorthToSouthLight = true;
            SouthToNorthLight = true;
            SouthToWestLight = true;
            SouthToEastLight = false;
        }
        if (Sequence == 4)
        {
            EastToNorthLight = true;
            EastToWestLight = true;
            EastToSouthLight = true;
            WestToNorthLight = true;
            WestToEastLight = true;
            WestToSouthLight = false;
            NorthToEastLight = true;
            NorthToWestLight = true;
            NorthToSouthLight = true;
            SouthToNorthLight = true;
            SouthToWestLight = false;
            SouthToEastLight = true;
        }
        if (Sequence == 5)
        {
            EastToNorthLight = false;
            EastToWestLight = true;
            EastToSouthLight = true;
            WestToNorthLight = true;
            WestToEastLight = true;
            WestToSouthLight = true;
            NorthToEastLight = false;
            NorthToWestLight = true;
            NorthToSouthLight = true;
            SouthToNorthLight = true;
            SouthToWestLight = true;
            SouthToEastLight = true;
        }
        yield return new WaitForSeconds(5);
        Change = true;
    }




    IEnumerator FourWaySwap()
    {
        Sequence = Random.Range(0, 11);

        if (Sequence == 0)
        { 
          EastLeftLightToNorth = false;
          EastLeftLightToWest = true;
          EastLeftLightToSouth = false;

          EastRightLightToNorth = false;
          EastRightLightToWest = true;
          EastRightLightToSouth = false;

          WestLeftLightToNorth = false;
          WestLeftLightToEast = true;
          WestLeftLightToSouth = false;

          WestRightLightToNorth = false;
          WestRightLightToEast = true;
          WestRightLightToSouth = false;




          NorthLeftLightToEast = false;
          NorthLeftLightToSouth = false;
          NorthLeftLightToWest = false;


          NorthRightLightToEast = false;
          NorthRightLightToSouth = false;
          NorthRightLightToWest = false;

          SouthLeftLightToWest = false;
          SouthLeftLightToNorth = false;
          SouthLeftLightToEast = false;

          SouthRightLightToWest = false;
          SouthRightLightToNorth = false;
          SouthRightLightToEast = false;


        }

        if (Sequence == 1)
        {
            EastLeftLightToNorth = false;
            EastLeftLightToWest = false;
            EastLeftLightToSouth = false;

            EastRightLightToNorth = false;
            EastRightLightToWest = false;
            EastRightLightToSouth = false;

            WestLeftLightToNorth = false;
            WestLeftLightToEast = false;
            WestLeftLightToSouth = false;

            WestRightLightToNorth = false;
            WestRightLightToEast = false;
            WestRightLightToSouth = false;


            NorthLeftLightToEast = false;
            NorthLeftLightToSouth = true;
            NorthLeftLightToWest = false;


            NorthRightLightToEast = false;
            NorthRightLightToSouth = true;
            NorthRightLightToWest = false;

            SouthLeftLightToWest = false;
            SouthLeftLightToNorth = true;
            SouthLeftLightToEast = false;

            SouthRightLightToWest = false;
            SouthRightLightToNorth = true;
            SouthRightLightToEast = false;
        }

        if (Sequence == 2)
        {
            EastLeftLightToNorth = false;
            EastLeftLightToWest = false;
            EastLeftLightToSouth = true;

            EastRightLightToNorth = false;
            EastRightLightToWest = false;
            EastRightLightToSouth = true;

            WestLeftLightToNorth = true;
            WestLeftLightToEast = false;
            WestLeftLightToSouth = false;

            WestRightLightToNorth = true;
            WestRightLightToEast = false;
            WestRightLightToSouth = false;


            NorthLeftLightToEast = true;
            NorthLeftLightToSouth = false;
            NorthLeftLightToWest = false;


            NorthRightLightToEast = true;
            NorthRightLightToSouth = false;
            NorthRightLightToWest = false;

            SouthLeftLightToWest = true;
            SouthLeftLightToNorth = false;
            SouthLeftLightToEast = false;

            SouthRightLightToWest = true;
            SouthRightLightToNorth = false;
            SouthRightLightToEast = false;
        }

        if (Sequence == 3)
        {
            EastLeftLightToNorth = false;
            EastLeftLightToWest = true;
            EastLeftLightToSouth = false;

            EastRightLightToNorth = false;
            EastRightLightToWest = true;
            EastRightLightToSouth = false;

            WestLeftLightToNorth = true;
            WestLeftLightToEast = false;
            WestLeftLightToSouth = false;

            WestRightLightToNorth = true;
            WestRightLightToEast = false;
            WestRightLightToSouth = false;


            NorthLeftLightToEast = true;
            NorthLeftLightToSouth = false;
            NorthLeftLightToWest = false;


            NorthRightLightToEast = true;
            NorthRightLightToSouth = false;
            NorthRightLightToWest = false;

            SouthLeftLightToWest = false;
            SouthLeftLightToNorth = false;
            SouthLeftLightToEast = false;

            SouthRightLightToWest = false;
            SouthRightLightToNorth = false;
            SouthRightLightToEast = false;
        }

        if (Sequence == 4)
        {
            EastLeftLightToNorth = false;
            EastLeftLightToWest = false;
            EastLeftLightToSouth = true;

            EastRightLightToNorth = false;
            EastRightLightToWest = false;
            EastRightLightToSouth = true;

            WestLeftLightToNorth = false;
            WestLeftLightToEast = true;
            WestLeftLightToSouth = false;

            WestRightLightToNorth = false;
            WestRightLightToEast = true;
            WestRightLightToSouth = false;


            NorthLeftLightToEast = false;
            NorthLeftLightToSouth = false;
            NorthLeftLightToWest = false;


            NorthRightLightToEast = false;
            NorthRightLightToSouth = false;
            NorthRightLightToWest = false;

            SouthLeftLightToWest = true;
            SouthLeftLightToNorth = false;
            SouthLeftLightToEast = false;

            SouthRightLightToWest = true;
            SouthRightLightToNorth = false;
            SouthRightLightToEast = false;
        }

        if (Sequence == 5)
        {
            EastLeftLightToNorth = false;
            EastLeftLightToWest = false;
            EastLeftLightToSouth = false;

            EastRightLightToNorth = false;
            EastRightLightToWest = false;
            EastRightLightToSouth = false;

            WestLeftLightToNorth = true;
            WestLeftLightToEast = false;
            WestLeftLightToSouth = false;

            WestRightLightToNorth = true;
            WestRightLightToEast = false;
            WestRightLightToSouth = false;


            NorthLeftLightToEast = false;
            NorthLeftLightToSouth = true;
            NorthLeftLightToWest = false;


            NorthRightLightToEast = false;
            NorthRightLightToSouth = true;
            NorthRightLightToWest = false;

            SouthLeftLightToWest = true;
            SouthLeftLightToNorth = false;
            SouthLeftLightToEast = false;

            SouthRightLightToWest = true;
            SouthRightLightToNorth = false;
            SouthRightLightToEast = false;
        }

        if (Sequence == 6)
        {
            EastLeftLightToNorth = false;
            EastLeftLightToWest = false;
            EastLeftLightToSouth = true;

            EastRightLightToNorth = false;
            EastRightLightToWest = false;
            EastRightLightToSouth = true;

            WestLeftLightToNorth = false;
            WestLeftLightToEast = false;
            WestLeftLightToSouth = false;

            WestRightLightToNorth = false;
            WestRightLightToEast = false;
            WestRightLightToSouth = false;


            NorthLeftLightToEast = true;
            NorthLeftLightToSouth = false;
            NorthLeftLightToWest = false;


            NorthRightLightToEast = true;
            NorthRightLightToSouth = false;
            NorthRightLightToWest = false;

            SouthLeftLightToWest = false;
            SouthLeftLightToNorth = true;
            SouthLeftLightToEast = false;

            SouthRightLightToWest = false;
            SouthRightLightToNorth = true;
            SouthRightLightToEast = false;
        }

        if (Sequence == 7)
        {
            EastLeftLightToNorth = false;
            EastLeftLightToWest = false;
            EastLeftLightToSouth = true;

            EastRightLightToNorth = false;
            EastRightLightToWest = false;
            EastRightLightToSouth = true;

            WestLeftLightToNorth = false;
            WestLeftLightToEast = false;
            WestLeftLightToSouth = false;

            WestRightLightToNorth = false;
            WestRightLightToEast = false;
            WestRightLightToSouth = false;


            NorthLeftLightToEast = false;
            NorthLeftLightToSouth = false;
            NorthLeftLightToWest = false;


            NorthRightLightToEast = false;
            NorthRightLightToSouth = false;
            NorthRightLightToWest = false;

            SouthLeftLightToWest = false;
            SouthLeftLightToNorth = false;
            SouthLeftLightToEast = true;

            SouthRightLightToWest = false;
            SouthRightLightToNorth = false;
            SouthRightLightToEast = true;
        }

        if (Sequence == 8)
        {
            EastLeftLightToNorth = false;
            EastLeftLightToWest = false;
            EastLeftLightToSouth = false;

            EastRightLightToNorth = false;
            EastRightLightToWest = false;
            EastRightLightToSouth = false;

            WestLeftLightToNorth = false;
            WestLeftLightToEast = false;
            WestLeftLightToSouth = true;

            WestRightLightToNorth = false;
            WestRightLightToEast = false;
            WestRightLightToSouth = true;


            NorthLeftLightToEast = false;
            NorthLeftLightToSouth = false;
            NorthLeftLightToWest = false;


            NorthRightLightToEast = false;
            NorthRightLightToSouth = false;
            NorthRightLightToWest = false;

            SouthLeftLightToWest = true;
            SouthLeftLightToNorth = false;
            SouthLeftLightToEast = false;

            SouthRightLightToWest = true;
            SouthRightLightToNorth = false;
            SouthRightLightToEast = false;
        }

        if (Sequence == 9)
        {
            EastLeftLightToNorth = false;
            EastLeftLightToWest = false;
            EastLeftLightToSouth = false;

            EastRightLightToNorth = false;
            EastRightLightToWest = false;
            EastRightLightToSouth = false;

            WestLeftLightToNorth = true;
            WestLeftLightToEast = false;
            WestLeftLightToSouth = false;

            WestRightLightToNorth = true;
            WestRightLightToEast = false;
            WestRightLightToSouth = false;


            NorthLeftLightToEast = false;
            NorthLeftLightToSouth = false;
            NorthLeftLightToWest = true;


            NorthRightLightToEast = false;
            NorthRightLightToSouth = false;
            NorthRightLightToWest = true;

            SouthLeftLightToWest = false;
            SouthLeftLightToNorth = false;
            SouthLeftLightToEast = false;

            SouthRightLightToWest = false;
            SouthRightLightToNorth = false;
            SouthRightLightToEast = false;
        }

        if (Sequence == 10)
        {
            EastLeftLightToNorth = true;
            EastLeftLightToWest = false;
            EastLeftLightToSouth = false;

            EastRightLightToNorth = true;
            EastRightLightToWest = false;
            EastRightLightToSouth = false;

            WestLeftLightToNorth = false;
            WestLeftLightToEast = false;
            WestLeftLightToSouth = false;

            WestRightLightToNorth = false;
            WestRightLightToEast = false;
            WestRightLightToSouth = false;


            NorthLeftLightToEast = true;
            NorthLeftLightToSouth = false;
            NorthLeftLightToWest = false;


            NorthRightLightToEast = true;
            NorthRightLightToSouth = false;
            NorthRightLightToWest = false;

            SouthLeftLightToWest = false;
            SouthLeftLightToNorth = false;
            SouthLeftLightToEast = false;

            SouthRightLightToWest = false;
            SouthRightLightToNorth = false;
            SouthRightLightToEast = false;
        }


        yield return new WaitForSeconds(5);
    Change = true;
    }

}
