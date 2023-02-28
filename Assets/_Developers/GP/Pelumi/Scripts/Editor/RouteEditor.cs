using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Routes))]
public class RouteEditor : Editor
{
    SerializedProperty listProperty;
    private void OnEnable()
    {
        listProperty = serializedObject.FindProperty("route");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("ownerID"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isLocal"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("debugTextSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("debugCircleSize"));

        Routes routes = (Routes)target;

        List<Vector3> routePositions = routes.GetRoutes;

        if (routePositions != null && routePositions.Count > 0)
        {
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                SerializedProperty element = listProperty.GetArrayElementAtIndex(i);

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    Undo.RecordObject(routes, "X");
                    routes.RemoveRouteAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                    return;
                }

                GUI.backgroundColor = Color.white;

                GUILayoutOption[] gUILayoutOptions = new GUILayoutOption[1];
                gUILayoutOptions[0] = GUILayout.Width(100);

                EditorGUILayout.LabelField("Route : " + (i + 1), gUILayoutOptions);

                EditorGUILayout.PropertyField(element, GUIContent.none);

                EditorGUILayout.EndHorizontal();
            }
        }


        Buttons(routes);
        serializedObject.ApplyModifiedProperties();
    }

    public void Buttons(Routes routes)
    {
        GUILayout.BeginHorizontal();

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add NewRoute"))
        {
            Undo.RecordObject(routes, "Remove Last Route");
            routes.AddNewRoute();
        }

        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Remove Last Route"))
        {
            Undo.RecordObject(routes, "Remove Last Route");
            routes.RemoveLastRoutes();
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Clear All"))
        {
            Undo.RecordObject(routes, "Clear All");
            routes.ClearRoutes();
        }

        GUILayout.EndHorizontal();
    }

    public void OnSceneGUI()
    {
        Routes routes = (Routes)target;

        List<Vector3> routePositions = routes.GetRoutes;

        Gizmos.color = Color.yellow;
        GUIStyle gUIStyle = new GUIStyle();
        gUIStyle.fontSize = routes.DebugTextSize;

        if (routePositions != null && routePositions.Count > 0)
        {
            for (int i = 0; i < routePositions.Count; i++)
            {
                Handles.Label(routes.GetRoute(i), routes.OwnerID + " Route" + (i + 1), gUIStyle);

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
