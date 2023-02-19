using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PackageSystem : MonoBehaviour
{
    #region GET & SET
    public int PackageScore => _packageScore;
    public int MaxPackages => _maxPackages;
    public int PackageAmount => _currentPackages.Count;

    #endregion

    [Header("Debug")]
    [Viewable] [SerializeField] private int _packageScore;
    [Viewable] [SerializeField] private int _packageAmount;

    [Header("Settings")]
    [SerializeField] private int _maxPackages;
    [SerializeField] private GameEvent _onPickUp;
    [SerializeField] private GameEvent _onDeliver;

    [Header("Visual")] 
    [SerializeField] private GameObject _packageObject;
    [SerializeField] private List<Vector3> _packageSpawns;
    [SerializeField] private UnityEvent _onDeliverEvent;

    private readonly List<GameObject> _currentPackageObjects = new List<GameObject>();
    private readonly List<PackageData> _currentPackages = new List<PackageData>();

    private void OnEnable() => DeliveryZone.OnDeliver += OnDeliver;
    private void OnDisable() => DeliveryZone.OnDeliver -= OnDeliver;
    private void Start() => _onPickUp.Raise(this, new int[] { _currentPackages.Count, _maxPackages });

    public void AddPackageData(PackageData packageData)
    {
        _currentPackages.Add(packageData);
        _onPickUp.Raise(this, new int[] { _currentPackages.Count, _maxPackages });
        AddPackageVisual();
        _packageAmount = PackageAmount;
    }
    
    public void RemovePackageData(PackageData packageData)
    {
        _currentPackages.Remove(packageData);
        _onPickUp.Raise(this, new int[] { _currentPackages.Count, _maxPackages });
    }

    public void ClearPackageData()
    {
        _currentPackages.Clear();
        _packageAmount = PackageAmount;
        ClearPackageVisuals();
    } 

    private void OnDeliver()
    {
        foreach (PackageData package in _currentPackages) { _packageScore += package.PackageScore; }
        ClearPackageData();
        _onDeliver.Raise(this, _packageScore);
        _onDeliverEvent?.Invoke();
    }

    private void AddPackageVisual()
    {
        if (!(_packageSpawns.Count >= _currentPackages.Count)) return;
        GameObject packageObject = Instantiate
            (_packageObject,transform.position + _packageSpawns[_currentPackages.Count - 1], Quaternion.identity);
        _currentPackageObjects.Add(packageObject);
        packageObject.transform.SetParent(transform);
        packageObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color =
            _currentPackages[^1].PackageColor;
    }

    private void ClearPackageVisuals()
    {
        foreach (GameObject packageObject in _currentPackageObjects) { Destroy(packageObject); }
        _currentPackageObjects.Clear();
    }
}

