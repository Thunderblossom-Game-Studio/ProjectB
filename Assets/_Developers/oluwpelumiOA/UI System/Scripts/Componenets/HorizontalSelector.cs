using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HorizontalSelector : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private List<string> options = new List<string>();
    [SerializeField] private TMP_Text selectedOptionDisplay;
    [SerializeField] private UnityEvent<int> OnValueChanged;
    [SerializeField] private Button leftButton, rightButton;

    [SerializeField] int m_index = 0;
    
    public int index
    {
        get { return m_index; }
        set
        {
            m_index = value;
            selectedOptionDisplay.text = options[m_index];
            OnValueChanged.Invoke(index);
        }
    }

    private void Start()
    {
        leftButton.onClick.AddListener(OnLeftClick);
        rightButton.onClick.AddListener(OnRightClick);
    }

    public void AddOption(string option)
    {
        options.Add(option);
    }

    public void SetOption(List<string> option)
    {
        options = option;
    }

    public void RemoveOption(string option)
    {
        options.Remove(option);
    }

    public void ClearOptions()
    {
        options.Clear();
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public void AddListener(UnityAction<int> action)
    {
        OnValueChanged.AddListener(action);
    }

    public void RemoveListener(UnityAction<int> action)
    {
        OnValueChanged.RemoveListener(action);
    }

    public void RemoveAllListeners()
    {
        OnValueChanged.RemoveAllListeners();
    }

    public TMP_Text GetText()
    {
        return selectedOptionDisplay;
    }

    public void OnLeftClick()
    {
        if (index == 0) index = options.Count - 1; else index--;
    }

    public void OnRightClick()
    {
        if ((index + 1) >= options.Count) index = 0; else index++;
    }
}
