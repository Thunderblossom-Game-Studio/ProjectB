using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vechicle/Resource")]
public class Resource : ScriptableObject
{
    public string _name;
    public float _maxAmount = 100;
    public float _startingAmount = 100;

    [Header("Properties")]
    public bool _canBeNegative = false;

    [HideInInspector] public float _amount;
    [HideInInspector] public UnityEngine.UI.Slider _resourceBar;
}