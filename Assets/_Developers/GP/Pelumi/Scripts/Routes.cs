using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Routes : MonoBehaviour
{
    [SerializeField] private string ownerID;
    [SerializeField] private int trainRouteDebugSize = 20;
    [SerializeField] private List<Vector3> route;

    public List<Vector3> GetRoutes() => route;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = trainRouteDebugSize;

        for (int i = 0; i < route.Count; i++)
        {
            Gizmos.DrawWireSphere(route[i], .2f);
            Handles.Label(route[i], ownerID + " Route" + (i + 1), gUIStyle);
        }

        Gizmos.color = Color.green;

        Vector3 startPos1 = route[0];

        for (int i = 0; i < route.Count; i++)
        {
            Gizmos.DrawLine(startPos1, route[i]);
            startPos1 = route[i];
        }

        Gizmos.DrawLine(route[0], route[^1]);
    }
}
