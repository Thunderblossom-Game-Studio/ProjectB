using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PackageSystem : MonoBehaviour
{
    #region GET & SET
    public int PackageScore => _packageScore;
    public int MaxPackages => _maxPackages;
    public int PackageAmount => _currentPackages.Count;

    #endregion

    [Viewable] [SerializeField] private int _packageScore;
    [SerializeField] private int _maxPackages;
    [SerializeField] private GameEvent _onPickUp;
    [SerializeField] private GameEvent _onDeliver;

    private readonly List<PackageData> _currentPackages = new List<PackageData>();

    private void OnEnable() => DeliveryZone.OnDeliver += OnDeliver;
    private void OnDisable() => DeliveryZone.OnDeliver -= OnDeliver;

    private void Start()
    {
        _onPickUp.Raise(this, new int[] { _currentPackages.Count, _maxPackages });
    }

    public void AddPackageData(PackageData packageData)
    {
        _currentPackages.Add(packageData);
        _onPickUp.Raise(this, new int[] { _currentPackages.Count, _maxPackages });
    }

    public void RemovePackageData(PackageData packageData)
    {
        _currentPackages.Remove(packageData);
        _onPickUp.Raise(this, new int[] { _currentPackages.Count, _maxPackages });
    }

    private void ClearPackageData() => _currentPackages.Clear();

    private void OnDeliver()
    {
        foreach (PackageData package in _currentPackages) { _packageScore += package.PackageScore; }
        ClearPackageData();
        _onDeliver.Raise(this, _packageScore);
    }
}

