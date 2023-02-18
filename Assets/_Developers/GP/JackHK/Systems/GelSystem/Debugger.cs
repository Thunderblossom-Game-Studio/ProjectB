using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    public Slider _debugFuelSlider;
    public Slider _debugBoostSlider;

    public GameObject _changeResourcePanel;
    public GameObject _changeResourceID;
    public GameObject _changeBurnrate;

    private int _resourceChange;
    private string _resourceID;
    private float _newBurnrate;

    private void Awake()
    {
        VechicleResources.Instance._resources[0]._resourceBar = _debugFuelSlider;
        VechicleResources.Instance._resources[1]._resourceBar = _debugBoostSlider;
    }
    public void ChangeBurnrate()
    {
        _newBurnrate = float.TryParse(_changeBurnrate.GetComponent<InputField>().text, out _newBurnrate) ? _newBurnrate : 0;
        VechicleResources.Instance._burnRate = _newBurnrate;
    }

    public void SetChangeResourceDebug()
    {
        Debug.Log("Amount Change: " + _resourceChange);
        Debug.Log("Resource: " + _resourceID);
        _resourceChange = int.TryParse(_changeResourcePanel.GetComponent<InputField>().text, out _resourceChange) ? _resourceChange : 0;
        _resourceID = _changeResourceID.GetComponent<InputField>().text;
        VechicleResources.Instance.IncreaseResource(_resourceID, _resourceChange);
    }
}