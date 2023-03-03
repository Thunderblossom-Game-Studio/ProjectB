using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] private Material materialToUse;

    public void ChangeMaterial()
    {
        ReplaceMaterials(materialToUse, gameObject);
    }

    public void ReplaceMaterials(Material newMaterial, GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out MeshRenderer meshRenderer)) meshRenderer.material = newMaterial;
        foreach (Transform child in gameObject.transform) ReplaceMaterials(newMaterial, child.gameObject);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(MaterialChanger))]
public class MaterialChangerEditor : Editor
{


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        DisplayButtons();
        serializedObject.ApplyModifiedProperties();
    }

    public void DisplayButtons()
    {
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("ChangeMaterial"))
        {
            (target as MaterialChanger).ChangeMaterial();
        }
    }
}

#endif