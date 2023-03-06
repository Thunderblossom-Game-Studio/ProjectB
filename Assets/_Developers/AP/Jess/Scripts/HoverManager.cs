using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HoverManager : MonoBehaviour
{
    public float xOffset;
    public float yOffset;

    public TextMeshProUGUI tipText;
    public RectTransform tipTextWindow;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;
    
    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }


    void Start()
    {
        HideTip();
    }

    private void ShowTip(string tip, Vector2 mouseposition)
    {
        tipText.text = tip;
        tipTextWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);
        tipTextWindow.gameObject.SetActive(true);
        //tipTextWindow.transform.position = new Vector2(mouseposition.x/2 + tipTextWindow.sizeDelta.x * 2, mouseposition.y);
        tipTextWindow.transform.position = new Vector2(mouseposition.x + xOffset, mouseposition.y + yOffset);
    }
    private void HideTip() 
    {
        tipText.text = default;
        tipTextWindow.gameObject.SetActive(false);
    }
    
 }
