using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Routes : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private string ownerID;
    [SerializeField] private bool islocal;
    [SerializeField] private List<Vector3> route;
  
    [Header("Debug")]
    [SerializeField] private int debugTextSize = 20;
    [SerializeField] private int debugCircleSize = 5;

    public string OwnerID => ownerID;
    public bool IsLoacl => islocal;
    public int DebugTextSize => debugTextSize;
    public int DebugCircleSize => debugCircleSize;

    public List<Vector3> GetRoutes() => route;

    public Vector3 GetRoute(int index)
    {
        return islocal ? route[index] + transform.position : route[index];
    }

    public void ClearRoutes()
    {
        route = new List<Vector3>();
    }

    public void RemoveLastRoutes()
    {
        if (route.Count > 0)  route.RemoveAt(route.Count - 1);
    }

    private void OnDrawGizmos()
    {
        if (route == null || route.Count == 0) return;

        Gizmos.color = Color.yellow;
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = debugTextSize;

        Vector3 startPos1 = GetRoute(0);

        for (int i = 0; i < route.Count; i++)
        {
            Gizmos.DrawWireSphere(GetRoute(i), debugCircleSize);
            Handles.Label(GetRoute(i), ownerID + " Route" + (i + 1), gUIStyle);
        }

        Gizmos.color = Color.green;

        for (int i = 0; i < route.Count; i++)
        {
            Gizmos.DrawLine(startPos1, GetRoute(i));
            startPos1 = GetRoute(i);
        }

        Gizmos.DrawLine(GetRoute(0), GetRoute(route.Count - 1));
    }
}
