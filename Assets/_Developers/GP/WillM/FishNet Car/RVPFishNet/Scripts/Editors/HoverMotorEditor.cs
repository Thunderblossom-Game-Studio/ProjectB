﻿#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace RVP
{
    [CustomEditor(typeof(HoverMotor))]
    [CanEditMultipleObjects]

    public class HoverMotorEditor : Editor
    {
        private bool _isPrefab = false;
        private static bool _showButtons = true;
        private float _topSpeed = 0;

        public override void OnInspectorGUI() {
            GUIStyle boldFoldout = new GUIStyle(EditorStyles.foldout);
            boldFoldout.fontStyle = FontStyle.Bold;
            HoverMotor targetScript = (HoverMotor)target;
            HoverMotor[] allTargets = new HoverMotor[targets.Length];
            _isPrefab = F.IsPrefab(targetScript);

            for (int i = 0; i < targets.Length; i++) {
                Undo.RecordObject(targets[i], "Hover Motor Change");
                allTargets[i] = targets[i] as HoverMotor;
            }

            _topSpeed = targetScript.forceCurve.keys[targetScript.forceCurve.keys.Length - 1].time;

            if (targetScript.wheels != null) {
                if (targetScript.wheels.Length == 0) {
                    EditorGUILayout.HelpBox("No wheels are assigned.", MessageType.Warning);
                }
                else if (targets.Length == 1) {
                    EditorGUILayout.LabelField("Top Speed (Estimate): " + (_topSpeed * 2.23694f).ToString("0.00") + " mph || " + (_topSpeed * 3.6f).ToString("0.00") + " km/h", EditorStyles.boldLabel);
                }
            }
            else {
                EditorGUILayout.HelpBox("No wheels are assigned.", MessageType.Warning);
            }

            DrawDefaultInspector();

            if (!_isPrefab && targetScript.gameObject.activeInHierarchy) {
                _showButtons = EditorGUILayout.Foldout(_showButtons, "Quick Actions", boldFoldout);
                EditorGUI.indentLevel++;
                if (_showButtons) {
                    if (GUILayout.Button("Get Wheels")) {
                        foreach (HoverMotor curTarget in allTargets) {
                            curTarget.wheels = curTarget.transform.GetTopmostParentComponent<VehicleParent>().transform.GetComponentsInChildren<HoverWheel>();
                        }
                    }
                }
                EditorGUI.indentLevel--;
            }

            if (GUI.changed) {
                EditorUtility.SetDirty(targetScript);
            }
        }
    }
}
#endif
