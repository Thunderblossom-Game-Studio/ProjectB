using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearCorrectionAINETTEST : MonoBehaviour
{
    public GameObject Point;
    public GameObject Base;
    public float step = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Point.transform.position, Base.transform.position) < 1)
        {
            Point.transform.position = Vector3.MoveTowards(Point.transform.position, Base.transform.position, step);
        }
    }

}
