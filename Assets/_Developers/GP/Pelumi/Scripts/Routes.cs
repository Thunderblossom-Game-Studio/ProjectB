using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Routes : MonoBehaviour
{
    [SerializeField] private string ownerID;
    [SerializeField] private bool isLocal;
    [SerializeField] private List<Vector3> route;
    [SerializeField] private int debugTextSize = 20;
    [SerializeField] private int debugCircleSize = 5;

    public string OwnerID => ownerID;
    public bool IsLoacl => isLocal;
    public int DebugTextSize => debugTextSize;
    public int DebugCircleSize => debugCircleSize;

    public List<Vector3> GetRoutes => route;

    public Vector3 GetRoute(int index)
    {
        return isLocal ? route[index] + transform.position : route[index];
    }

    public void AddNewRoute()
    {
        Vector3 newRoute = (route.Count == 0) ? transform.position + (Vector3.forward * 5) : route[^1] + (Vector3.forward * 5);
        route.Add(newRoute);
    }

    public void ClearRoutes()
    {
        route = new List<Vector3>();
    }

    public void RemoveLastRoutes()
    {
        if (route.Count > 0)  route.RemoveAt(route.Count - 1);
    }

    public void RemoveRouteAtIndex(int index)
    {
        route.RemoveAt(index);
    }

    private void OnDrawGizmos()
    {
        if (route == null || route.Count == 0) return;

        Gizmos.color = Color.yellow;
        Vector3 startPos1 = GetRoute(0);

        for (int i = 0; i < route.Count; i++) Gizmos.DrawWireSphere(GetRoute(i), debugCircleSize);

        Gizmos.color = Color.green;

        for (int i = 0; i < route.Count; i++)
        {
            Gizmos.DrawLine(startPos1, GetRoute(i));
            startPos1 = GetRoute(i);
        }

        Gizmos.DrawLine(GetRoute(0), GetRoute(route.Count - 1));
    }
}
