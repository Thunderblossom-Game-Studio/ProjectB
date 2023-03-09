using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RenameTool : EditorWindow
{
    private string newName;

    [MenuItem("Window/Custom Tools/RenameTool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(RenameTool));
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

        newName = GUILayout.TextField(newName);

        if (Selection.gameObjects.Length > 0)
        {
            GUI.backgroundColor = Color.red;
            GUILayout.BeginVertical("box");
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                GUILayout.Label(Selection.objects[i].name);
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndVertical();
        }

        GUILayout.Space(5);
        GUILayout.BeginHorizontal("box");

        if (GUILayout.Button("RenameAll"))
        {
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                Selection.objects[i].name = newName + "_" + (i + 1);
            }
        }

        GUILayout.EndHorizontal();
    }
}
