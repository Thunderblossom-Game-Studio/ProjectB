using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{

    public float Distance = 10;

    // Update is called once per frame
    void Update()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = r.GetPoint(Distance);
        transform.position = pos;
    }
}
