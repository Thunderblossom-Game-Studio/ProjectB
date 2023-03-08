//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GiveWayJunctionAINETTEST : MonoBehaviour
//{
//    public GameObject RayOne;
//    public GameObject RayTwo;
//    public GameObject GiveWay;
//    public GameObject GiveWayTwo;

//    void FixedUpdate()
//    {
//        RaycastHit hit;
//        RaycastHit hit2;
//        Vector3 forward = RayOne.transform.TransformDirection(Vector3.up) * 10;
//        if (Physics.Raycast(RayOne.transform.position, forward, out hit, 5.0f))
//        {
//            if (hit.rigidbody != null)
//            {
//                GiveWay.GetComponent<WaypointControlAINETTEST>().Red = true;
//                StartCoroutine("Hold");
//            }
//        }
//        Vector3 forward2 = RayTwo.transform.TransformDirection(Vector3.up) * 10;
//        if (Physics.Raycast(RayTwo.transform.position, forward2, out hit2, 5.0f))
//        {

//            if (hit2.rigidbody != null)
//            {
//                GiveWay.GetComponent<WaypointControlAINETTEST>().Red = true;
//                GiveWayTwo.GetComponent<WaypointControlAINETTEST>().Red = true;
//                StartCoroutine("Hold2");
//                StartCoroutine("Hold");
//            }
//        }

//    }
//    IEnumerator Hold()
//    {
//        yield return new WaitForSeconds(5);
//        GiveWay.GetComponent<WaypointControlAINETTEST>().Red = false;
//    }
//    IEnumerator Hold2()
//    {
//        yield return new WaitForSeconds(5);
//        GiveWay.GetComponent<WaypointControlAINETTEST>().Red = false;
//        GiveWayTwo.GetComponent<WaypointControlAINETTEST>().Red = false;
//    }
//}
