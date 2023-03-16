using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "New Audio Bank", menuName = "Audio/Audio Bank")]
public class AudioBank : ScriptableObject
{
   [SerializeField] private List<AudioWithID> audioClips = new List<AudioWithID>();
    
    public AudioClip GetAudioByID(string ID)
    {
        AudioWithID audioWithID = audioClips.Find(x => x.ID == ID);
        if (audioWithID == null)
        {
            Debug.LogError("Audio with ID: " + ID + " not found in " + name);
            return null;
        }

        if (audioWithID.hasSimilar && audioWithID.similarAudioClips.Length > 0 && audioWithID.similarAudioClips[0] != null)
        {
            if (Random.value > 0.5f) return audioWithID.audioClip;
            else  return audioWithID.similarAudioClips[Random.Range(0, audioWithID.similarAudioClips.Length)];        
        }
        else
        {
            return audioWithID.audioClip;
        }
    }

    public AudioClip GetRandomClip()
    {
        return audioClips[Random.Range(0, audioClips.Count)].audioClip;
    }
}

[System.Serializable]
public class AudioWithID
{
    public string ID;
    public AudioClip audioClip;
    public string info;
    public bool hasSimilar;
    public AudioClip[] similarAudioClips;
}

#if UNITY_EDITOR

[CustomEditor(typeof(AudioBank))]
public class AudioWithIDEditor : Editor
{
    SerializedProperty listProperty;
    GUILayoutOption[] verticalBoxOption;

    private void OnEnable()
    {
        listProperty = serializedObject.FindProperty("audioClips");
        verticalBoxOption = new GUILayoutOption[] { GUILayout.MaxHeight(50) };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DisplayAudioList();
        DisplayButtons();
        serializedObject.ApplyModifiedProperties();
        CheckForDuplicates();
        serializedObject.ApplyModifiedProperties();
    }

    public void DisplayAudioList()
    {
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
            
            GUI.backgroundColor = (i % 2 == 0) ? Color.blue : Color.white;

            if (element.FindPropertyRelative("audioClip").objectReferenceValue == null) GUI.backgroundColor = Color.red;

            EditorGUILayout.BeginVertical("box", verticalBoxOption);

            EditorGUILayout.BeginHorizontal("box");

            EditorGUILayout.LabelField(i.ToString());

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                listProperty.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            EditorGUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;

            EditorGUILayout.PropertyField(element.FindPropertyRelative("ID"), new GUIContent("ID", "Put ID Here"));
            EditorGUILayout.PropertyField(element.FindPropertyRelative("audioClip"), new GUIContent("AudioClip", "Put AudioClip Here"));
            EditorGUILayout.PropertyField(element.FindPropertyRelative("info"), new GUIContent("Info", "Put Info Here"), new GUILayoutOption[] { GUILayout.Height(50) });

            SerializedProperty serializedProperty = listProperty.GetArrayElementAtIndex(i);

            serializedProperty.FindPropertyRelative("hasSimilar").boolValue = EditorGUILayout.Toggle("Has Similar", serializedProperty.FindPropertyRelative("hasSimilar").boolValue);
            if (serializedProperty.FindPropertyRelative("hasSimilar").boolValue) EditorGUILayout.PropertyField(element.FindPropertyRelative("similarAudioClips"), new GUIContent("Similar AudioClips", "Put Similar AudioClips Here"));

            EditorGUILayout.EndVertical();
            GUILayout.Space(10);
        }
    }

    public void DisplayButtons()
    {
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add Audio Clip"))
        {
            listProperty.arraySize++;
            serializedObject.ApplyModifiedProperties();
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Remove last Audio Clip"))
        {
            listProperty.arraySize--;
            serializedObject.ApplyModifiedProperties();
        }
    }

    
    public void CheckForDuplicates()
    {
        for (int i = 0; i < listProperty.arraySize; i++)
        {
            SerializedProperty element = listProperty.GetArrayElementAtIndex(i);
            string id = element.FindPropertyRelative("ID").stringValue;
            for (int j = 0; j < listProperty.arraySize; j++)
            {
                if (i == j) continue;
                SerializedProperty element2 = listProperty.GetArrayElementAtIndex(j);
                string id2 = element2.FindPropertyRelative("ID").stringValue;
                if (id == id2)
                {
                    EditorGUILayout.HelpBox("Audio with same ID already exists", MessageType.Error);
                    return;
                }
            }
        }
    }
}
#endif