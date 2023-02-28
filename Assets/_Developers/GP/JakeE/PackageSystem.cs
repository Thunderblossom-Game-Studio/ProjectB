using System;
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
    public Action OnPickUp { get => _onPickUp; set => _onPickUp = value; }
    public Action OnDeliver { get => _onDeliver; set => _onDeliver = value; }

    #endregion

    [Header("Debug")]
    [Viewable] [SerializeField] private int _packageScore;

    [Header("Settings")] 
    [SerializeField] private GameObject _packageDrop;
    [SerializeField] private GameObject _packageObjectVisual;
    [SerializeField] private int _maxPackages;
    [SerializeField] private List<Vector3> _packageSpawns;
    [SerializeField] private UnityEvent _onDeliverEvent;

    private readonly List<GameObject> _currentPackageObjects = new List<GameObject>();
    private readonly List<PackageData> _currentPackages = new List<PackageData>();
    private Action _onPickUp;
    private Action _onDeliver;
    
    private void Start() => _onPickUp.Invoke();

    public void AddPackageData(PackageData packageData)
    {
        _currentPackages.Add(packageData);
        OnPickUp?.Invoke();
        AddPackageVisual();
    }
    
    public void RemovePackageData(PackageData packageData)
    {
        _currentPackages.Remove(packageData);
        _onPickUp?.Invoke();
    }

    public void ClearPackageData()
    {
        _currentPackages.Clear();
        ClearPackageVisuals();
    } 

    public void DeliverPackages()
    {
        int totalScore = 0;
        int totalPackage = 0;

        foreach (PackageData package in _currentPackages)
        {
            totalScore += package.PackageScore;
            totalPackage++;
        }

        //To Change >>>
        _packageScore += totalScore;
        if (!TryGetComponent(out GamePlayer gamePlayer)) return;
        if (GameStateManager.Instance)
        {
            GameStateManager.Instance.TeamManager.AddScore(gamePlayer.PlayerTeamData, totalScore, totalPackage);
        }
        //To Change <<<
        
        ClearPackageData();
        _onDeliver?.Invoke();
        _onDeliverEvent?.Invoke();
    }

    private void AddPackageVisual()
    {
        if (!(_packageSpawns.Count >= _currentPackages.Count)) return;
        GameObject packageObject = Instantiate
            (_packageObjectVisual,transform.position + _packageSpawns[_currentPackages.Count - 1], Quaternion.identity);
        _currentPackageObjects.Add(packageObject);
        packageObject.transform.SetParent(transform);
        packageObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color =
            _currentPackages[^1].PackageColor;
        packageObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        packageObject.transform.localPosition = _packageSpawns[_currentPackages.Count - 1];
    }

    private void DropPackages()
    {
        foreach (PackageData package in _currentPackages)
        {
            GameObject droppedPackage =
                Instantiate(_packageDrop, transform.position + (Vector3.up * 2), Quaternion.identity);
        }
    }

    private void ClearPackageVisuals()
    {
        foreach (GameObject packageObject in _currentPackageObjects) { Destroy(packageObject); }
        _currentPackageObjects.Clear();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_packageSpawns.Count < 1) return;
        foreach (Vector3 packageLocation in _packageSpawns)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawCube(transform.position + packageLocation, _packageObjectVisual.transform.localScale);
        }
    }
#endif
}

