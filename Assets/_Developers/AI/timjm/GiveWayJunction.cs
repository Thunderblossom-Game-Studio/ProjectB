using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveWayJunction : MonoBehaviour
{
    public GameObject RayOne;
    public GameObject RayTwo;
    public GameObject RayThree;
    public GameObject RayFour;

    public GameObject GiveWay;
    public GameObject GiveWayTwo;

    [SerializeField] bool FourLaneJunction;

    void FixedUpdate()
    {
        RaycastHit hit;
        RaycastHit hit2;
        Vector3 forward = RayOne.transform.TransformDirection(Vector3.up) * 10;

        if (FourLaneJunction)
        {
         Vector3 forward3 = RayThree.transform.TransformDirection(Vector3.up) * 10;
        }

        if (Physics.Raycast(RayOne.transform.position, forward, out hit, 5.0f) || Physics.Raycast(RayThree.transform.position,forward3,out hit, 5.0f))
        {
            if (hit.rigidbody != null)
            {
                GiveWay.GetComponent<WaypointControl>().Red = true;
                StartCoroutine("Hold");
            }
        }

        Vector3 forward2 = RayTwo.transform.TransformDirection(Vector3.up) * 10;

        if (FourLaneJunction) 
        {         
            Vector3 forward4 = RayFour.transform.TransformDirection(Vector3.up) * 10;
        }

        if (Physics.Raycast(RayTwo.transform.position, forward2, out hit2, 5.0f) || Physics.Raycast(RayFour.transform.position, forward4, out hit, 5.0f))
        {
            
            if (hit2.rigidbody != null)
            {
                GiveWay.GetComponent<WaypointControl>().Red = true;
                GiveWayTwo.GetComponent<WaypointControl>().Red = true;
                StartCoroutine("Hold2");
                StartCoroutine("Hold");
            }
        }

    }
    IEnumerator Hold()
    {
        yield return new WaitForSeconds(5);
        GiveWay.GetComponent<WaypointControl>().Red = false;
    }
    IEnumerator Hold2()
    {
        yield return new WaitForSeconds(5);
        GiveWay.GetComponent<WaypointControl>().Red = false;
        GiveWayTwo.GetComponent<WaypointControl>().Red = false;
    }
}
