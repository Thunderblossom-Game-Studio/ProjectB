using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BindUI : MonoBehaviour
{
    [SerializeField] private Button bindButton;
    [SerializeField]  private TextMeshProUGUI bindText;

    public void SetBindText(string text)
    {
        bindText.text = text;
    }

    public void SetBindButton(Action onClick)
    {
        bindButton.onClick.AddListener(() => onClick());
    }
}
