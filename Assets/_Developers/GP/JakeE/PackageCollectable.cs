using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageCollectable : Collectable
{
    [SerializeField] private PackageData _packageData;

    protected override void Collect(GameObject collideObject)
    {
        PackageSystem packageSystem = collideObject.GetComponent<PackageSystem>();
        if (packageSystem == null) return;
        if (packageSystem.PackageAmount >= packageSystem.MaxPackages) return;
        packageSystem.AddPackageData(_packageData);
        _onCollect?.Invoke();
        DestroyObject();
    }
}

[Serializable]
public struct PackageData
{
    #region GET & SET

    public int PackageWeight => _packageWeight;
    public int PackageScore => _packageScore;
    public Color PackageColor => _packageVisualColor;

    #endregion

    [SerializeField] private Color _packageVisualColor;
    [SerializeField] private int _packageWeight;
    [SerializeField] private int _packageScore;
}
