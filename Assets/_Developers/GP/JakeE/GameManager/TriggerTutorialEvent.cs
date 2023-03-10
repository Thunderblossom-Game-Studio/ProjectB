using JE.General;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class TriggerTutorialEvent : MonoBehaviour
{
    [SerializeField] private TutorialStateManager.TutorialAttribute _attributeToModify;

    public void Trigger()
    {
        Debug.Log("Trigger");
        TutorialStateManager.Instance.CompleteAttribute(_attributeToModify);
    }
}
