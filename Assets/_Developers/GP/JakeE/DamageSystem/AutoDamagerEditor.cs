using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CustomEditor(typeof(AutoDamager))]
public class AutoDamagerEditor : Editor
{
    private const string BUTTON_ENABLE_TEXT = "ENABLE DAMAGER";
    private const string BUTTON_DISABLE_TEXT = "DISABLE DAMAGER";
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        EnableDamagerButton();
        DisableDamagerButton();
    }

    private void EnableDamagerButton()
    {
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button(BUTTON_ENABLE_TEXT))
        {
            AutoDamager autoDamager = (AutoDamager)target;
            autoDamager.StartDamager();
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void DisableDamagerButton()
    {
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button(BUTTON_DISABLE_TEXT))
        {
            AutoDamager autoDamager = (AutoDamager)target;
            autoDamager.StopDamager();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif