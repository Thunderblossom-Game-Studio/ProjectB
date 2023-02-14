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
    //Debug >>>
    [SerializeField] private TextMeshProUGUI _packageScoreText;
    //Debug <<<

    [Viewable] [SerializeField] private int _packageScore;
    [SerializeField] private int _maxPackages;
    private readonly List<PackageData> _currentPackages = new List<PackageData>();

    private void OnEnable() => DeliveryZone.OnDeliver += OnDeliver;
    private void OnDisable() => DeliveryZone.OnDeliver -= OnDeliver;

    public void AddPackageData(PackageData packageData) => _currentPackages.Add(packageData);
    public void RemovePackageData(PackageData packageData) => _currentPackages.Remove(packageData);
    private void ClearPackageData() => _currentPackages.Clear();

    private void OnDeliver()
    {
        foreach (PackageData package in _currentPackages) { _packageScore += package.PackageScore; }
        ClearPackageData();
        
        //Debug <<<
        if (!_packageScoreText) return;
        _packageScoreText.text = _packageScore.ToString();
        //Debug >>>
    }
    

}

