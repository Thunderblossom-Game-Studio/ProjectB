using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabToggle : Toggle
{
    [SerializeField] private Selectable firstSelectedButton;
    [SerializeField] private GameObject contentMenu;

    public Selectable GetSelectable() => firstSelectedButton;

    public void OnToggled(bool isOn)
    {
        contentMenu.SetActive(isOn);
        if (isOn)
        {
            //if (InputManager.Instance.GetCurrentDeviceType() == InputManager.DeviceType.Gamepad)
            Focus();
        } 
    }

    public void Focus()
    {
        firstSelectedButton?.Select();
    }
}
