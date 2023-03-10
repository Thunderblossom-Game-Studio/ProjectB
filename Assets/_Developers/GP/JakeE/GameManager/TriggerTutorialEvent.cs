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
        TutorialStateManager.Instance.CompleteAttribute(_attributeToModify);
    }
}

public enum MyEnum
{
    Option1,
    Option2,
    Option3
}

