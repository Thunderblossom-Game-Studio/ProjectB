using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PolygonCounter : EditorWindow
{
    private int selection = 0;
    private float polygonCount;
    private float totalPolygonCount;

    [MenuItem("Window/Custom Tools/PolygonCounter")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PolygonCounter));
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Tool Made by Oluwapelumi Ayeni");
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal("box");
        GUILayout.Label("Selected GameObject : " + Selection.gameObjects.Length);
        GUILayout.EndHorizontal();

        if (Selection.objects.Length != selection)
        {
            selection = Selection.objects.Length;
        }

        for (int i = 0; i < Selection.objects.Length; i++)
        {
            GUILayout.BeginHorizontal("box");
            GUILayout.Label(Selection.objects[i].name);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(5);
        GUILayout.BeginHorizontal("box");

        GameObject[] selectedGameobjects = Selection.gameObjects;

        if (selectedGameobjects.Length > 0)
        {
            polygonCount = 0.0f;
            totalPolygonCount = 0.0f;

            foreach (GameObject gameObject in selectedGameobjects)
            {
                if (gameObject.GetComponent<MeshFilter>())
                {
                    polygonCount = gameObject.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
                    totalPolygonCount += polygonCount;
                }

            }
        }
        else
        {
            polygonCount = 0.0f;
            totalPolygonCount = 0.0f;
        }

        GUILayout.Label("Polygon Count = " + totalPolygonCount);
        GUILayout.EndHorizontal();
    }


    private static void FindInSelected()
    {
        GameObject[] selectedGameobjects = Selection.gameObjects;

        foreach (GameObject gameObject in selectedGameobjects)
        {
            
        }
    }
}
