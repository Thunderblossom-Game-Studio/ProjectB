using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialReplacement : EditorWindow
{
    private int selection = 0;
    private float polygonCount;
    public Material materialInUse;


 [MenuItem("Window/Custom Tools/MaterialReplacer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MaterialReplacement));

     
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

        //SerializedObject serializedObject = new SerializedObject(materialInUse);
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("materialInUse"));

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


        if(GUILayout.Button("ReplaceAllMaterials"))
        {
            if (selectedGameobjects.Length > 0)
            {
                ReplaceAllMaterials(materialInUse, selectedGameobjects);
            }
        }

        GUILayout.EndHorizontal();
    }


    private void ReplaceAllMaterials(Material newMaterial, GameObject[] selectedGameobjects)
    {
        foreach (GameObject gameObject in selectedGameobjects)
        {
            ReplaceMaterials(newMaterial, gameObject);
        }
    }

    public void ReplaceMaterials(Material newMaterial, GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out MeshRenderer meshRenderer)) meshRenderer.material = newMaterial;
        foreach (Transform child in gameObject.transform) ReplaceMaterials(newMaterial, child.gameObject);
    }
}
