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
}
