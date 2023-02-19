using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(AdvanceButton))]
[CanEditMultipleObjects]
public class AdvancedButtonEditor : ButtonEditor
{
    private SerializedProperty _OnMouseOverProperty;
    private SerializedProperty _OnMouseExitProperty;

    protected override void OnEnable()
    {
        base.OnEnable();
        _OnMouseOverProperty = serializedObject.FindProperty("onMouseOver");
        _OnMouseExitProperty = serializedObject.FindProperty("onMouseExit");
    }

    public override void OnInspectorGUI()
    {
        AdvanceButton targetMyButton = (AdvanceButton)target;

        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(_OnMouseOverProperty);
        EditorGUILayout.PropertyField(_OnMouseExitProperty);
        serializedObject.ApplyModifiedProperties();
    }
}
