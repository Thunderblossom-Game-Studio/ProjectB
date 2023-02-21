using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SaveLoadWindow : EditorWindow
{
    private string saveName;
    private bool encypted;

    [MenuItem("Window/Custom Tools/SaveLoadHelper")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SaveLoadWindow));
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
        GUILayout.TextField("Save Name");
        saveName = GUILayout.TextField(saveName);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal("box");
        GUILayout.TextField("Encrypted");
        encypted = GUILayout.Toggle(encypted, "");
        GUILayout.EndHorizontal();
        

        GUILayout.Space(10);

        GUILayout.BeginHorizontal("box");

        if (GUILayout.Button("Delete"))
        {
             SaveLoadManager.Delete(saveName);
        }
        
        GUILayout.EndHorizontal();
    }
}
