using System;
using UnityEngine;

public class PackageCollectable : Collectable
{
    [SerializeField] private PackageData _packageData;

    protected override void Collect(GameObject collideObject)
    {
        if (!collideObject.TryGetComponent(out PackageSystem packageSystem))
        {
            if (collideObject.transform.parent == null) return;
            GameObject targetObject = collideObject.transform.parent.gameObject;
            if (!targetObject.TryGetComponent(out packageSystem))
            {
                return;
            }
        }

        if (packageSystem.PackageAmount >= packageSystem.MaxPackages)
            return;

        packageSystem.AddPackageData(_packageData);
        _onCollect?.Invoke();
        DestroyObject();
    }

}

[Serializable]
public struct PackageData
{
    #region GET & SET

    public PackageType PackageType => _packageType;
    public int PackageScore => _packageScore;

    #endregion

    [SerializeField] private PackageType _packageType;
    [SerializeField] private int _packageScore;
}

public enum PackageType
{
    Normal,
    Rare
}