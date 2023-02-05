using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficDirector : MonoBehaviour
{
    public GameObject ExitOne;
    public GameObject ExitTwo;
    public GameObject ExitThree;
    public GameObject ExitFour;
    public float NextMarker;
    public GameObject Next;
    public float ExitNumber;
    public bool MultiExit;
    public GameObject Car;

    public void Lane()
    {
        if( MultiExit == true )
        {
            NextMarker = Random.Range(0f, ExitNumber);
            NextMarker = (Mathf.Round(NextMarker));
            if(NextMarker >= ExitNumber)
            {
                NextMarker = ExitNumber - 1; 
            }
            if (NextMarker == 0)
            {
                Next = ExitOne;
            }
            if (NextMarker == 1)
            {
                Next = ExitTwo;
            }
            if (NextMarker == 2)
            {
                Next = ExitThree;
            }
            if (NextMarker == 3)
            {
                Next = ExitFour;
            }
        }
        Car.GetComponent<TrafficTrigger>().Target = Next;
    }
}
