using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Routes))]
public class RouteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        Routes routes = (Routes)target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear All"))
        {
            Undo.RecordObject(routes, "Clear All");
            routes.ClearRoutes();
        }

        if (GUILayout.Button("Remove Last Route"))
        {
            Undo.RecordObject(routes, "Remove Last Route");
            routes.RemoveLastRoutes();
        }
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        Routes routes = (Routes)target;

        List<Vector3> routePositions = routes.GetRoutes();

        if (routePositions != null && routePositions.Count > 0)
        {
            for (int i = 0; i < routePositions.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 newPosition = Handles.PositionHandle(routes.GetRoute(i), Quaternion.identity);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(routes, "Change Anchor Position");
                    routePositions[i] = newPosition;
                    serializedObject.Update();
                }
            }
        }
    }
}
