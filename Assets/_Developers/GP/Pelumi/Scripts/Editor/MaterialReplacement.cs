using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialReplacement : EditorWindow
{
    private Material materialInUse;

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

        materialInUse = (Material)EditorGUILayout.ObjectField(new GUIContent("Material", "Material to assign to all objects and children"), materialInUse, typeof(Material), false);

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

        if(GUILayout.Button("ReplaceAllMaterials"))
        {
            if (Selection.gameObjects.Length > 0) ReplaceAllMaterials(materialInUse, Selection.gameObjects);
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
