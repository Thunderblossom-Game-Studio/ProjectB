using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorialEvent : MonoBehaviour
{
    [SerializeField] private TutorialStateManager.TutorialAttribute _attributeToModify;

    public void Trigger()
    {
        TutorialStateManager.Instance.CompleteAttribute(_attributeToModify);
    }
}
