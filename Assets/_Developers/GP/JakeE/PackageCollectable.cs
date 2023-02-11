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
        packageSystem.AddPackageData(_packageData);
        Destroy(gameObject);
    }
}

[Serializable]
public struct PackageData
{
    #region GET & SET

    public int PackageWeight => _packageWeight;
    public int PackageScore => _packageScore;

    #endregion

    [SerializeField] private int _packageWeight;
    [SerializeField] private int _packageScore;
}
