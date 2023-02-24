using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PackageSystem))]
public class PlayerPackageHandler : MonoBehaviour
{
    [SerializeField] private GameEvent _onPickUp;
    [SerializeField] private GameEvent _onDeliver;
    private PackageSystem _packageSystem;

    private void Awake() => _packageSystem = GetComponent<PackageSystem>();
    
    private void OnEnable()
    {
        _packageSystem.OnDeliver += DeliverPackage;
        _packageSystem.OnPickUp += CollectPackage;
    }

    private void OnDisable()
    {
        _packageSystem.OnDeliver -= DeliverPackage;
        _packageSystem.OnPickUp -= CollectPackage;
    }

    private void CollectPackage() => _onPickUp.Raise(this, new int[] { _packageSystem.PackageAmount, _packageSystem.MaxPackages });

    private void DeliverPackage() => _onDeliver.Raise(this, _packageSystem.PackageScore);


}
