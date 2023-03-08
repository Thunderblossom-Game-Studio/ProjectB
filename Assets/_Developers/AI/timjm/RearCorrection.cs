using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearCorrection : MonoBehaviour
{
    public GameObject RovingPoint;
    public GameObject ControlPoint;
    public float step = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(RovingPoint.transform.position, ControlPoint.transform.position) < 1)
        {
            RovingPoint.transform.position = Vector3.MoveTowards(RovingPoint.transform.position, ControlPoint.transform.position, step);
        }
    }
    
}
