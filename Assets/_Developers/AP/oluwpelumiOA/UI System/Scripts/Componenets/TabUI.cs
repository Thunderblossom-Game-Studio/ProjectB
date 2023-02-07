using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TabUI : MonoBehaviour
{
    public enum Direction { Left = -1,Right = 1 }

    [SerializeField] private TabToggle[] controlToggle;
    
    public void SelectLeft()
    {
        HandleToogle(Direction.Left);
    }

    public void SelectRight()
    {
        HandleToogle(Direction.Right);
    }

    private void HandleToogle(Direction direction)
    {
        TabToggle activeToggle = controlToggle.FirstOrDefault(t => t.isOn);

        if (direction == Direction.Right)
        {
            int nextIndex = Array.IndexOf(controlToggle, activeToggle) + 1;
            if (nextIndex < controlToggle.Length)
            {
                controlToggle[nextIndex].isOn = true;
            }
            else
            {
                controlToggle[0].isOn = true;
            }
        }
        else
        {
            int nextIndex = Array.IndexOf(controlToggle, activeToggle) - 1;
            if (nextIndex >= 0)
            {
                controlToggle[nextIndex].isOn = true;
            }
            else
            {
                controlToggle[controlToggle.Length - 1].isOn = true;
            }
        }
    }

    public void ReFocus()
    {
        TabToggle activeToggle = controlToggle.FirstOrDefault(t => t.isOn);
        activeToggle.Focus();
    }

    public Selectable GetCurrentTabTogleSelectable()
    {
        return controlToggle.FirstOrDefault(t => t.isOn).GetSelectable();
    }
}
