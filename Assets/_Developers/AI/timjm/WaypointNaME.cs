using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNaME : MonoBehaviour
{
    public Transform t;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        TextMesh textMesh = t.GetComponent(typeof(TextMesh)) as TextMesh;
        textMesh.text = transform.name;

    }
}
