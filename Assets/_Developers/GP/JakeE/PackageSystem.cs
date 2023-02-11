using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PackageSystem : MonoBehaviour
{
    // >>>>>> (Temp / Debug)
    [Viewable] [SerializeField] private int _tempScore;
    // <<<<<<
    
    [SerializeField] private int _maxPackages;
    private readonly List<PackageData> _currentPackages = new List<PackageData>();

    private void OnEnable() => DeliveryZone.OnDeliver += OnDeliver;
    private void OnDisable() => DeliveryZone.OnDeliver -= OnDeliver;

    public void AddPackageData(PackageData packageData) => _currentPackages.Add(packageData);
    public void RemovePackageData(PackageData packageData) => _currentPackages.Remove(packageData);

    private void OnDeliver()
    {
        foreach (PackageData package in _currentPackages) { _tempScore += package.PackageScore; }
        Debug.Log("Package Delivered!");
    }
    

}

