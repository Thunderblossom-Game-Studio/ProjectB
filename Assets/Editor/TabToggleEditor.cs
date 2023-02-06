using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(TabToggle))]
public class TabToggleEditor : ToggleEditor
{
    private SerializedProperty _firstSelectedButton;
    private SerializedProperty _contentMenu;

    protected override void OnEnable()
    {
        base.OnEnable();
        _firstSelectedButton = serializedObject.FindProperty("firstSelectedButton");
        _contentMenu = serializedObject.FindProperty("contentMenu");
    }

    public override void OnInspectorGUI()
    {
        TabToggle targetMyButton = (TabToggle)target;

        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(_firstSelectedButton);
        EditorGUILayout.PropertyField(_contentMenu);
        serializedObject.ApplyModifiedProperties();
    }
}
