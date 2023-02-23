using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private GameEvent _onOnAmmoChanged;
    [SerializeField] private GameEvent _onReloadStart;
    [SerializeField] private GameEvent _onReloading;
    [SerializeField] private GameEvent _onReloadEnd;
    
    private WeaponHandler weaponHandler;

    private void Awake()
    {
        weaponHandler = GetComponent<WeaponHandler>();
    }

    private void OnEnable()
    {
        weaponHandler.OnAmmoChanged += WeaponHandler_OnAmmoChanged;
        weaponHandler.OnReloadStart += WeaponHandler_OnReloadStart;
        weaponHandler.OnReloading += WeaponHandler_OnReloadDuration;
        weaponHandler.OnReloadEnd += WeaponHandler_OnReloadEnd;
    }

    private void WeaponHandler_OnAmmoChanged(object sender, System.EventArgs e)
    {
        _onOnAmmoChanged.Raise(this, new int[] { weaponHandler.CurrentAmmo, weaponHandler.MaxAmmo });
    }

    private void WeaponHandler_OnReloadStart(object sender, System.EventArgs e)
    {
        _onReloadStart.Raise(this, null);
    }

    private void WeaponHandler_OnReloadDuration(object sender, float value)
    {
        _onReloading.Raise(this, value);
    }

    private void WeaponHandler_OnReloadEnd(object sender, EventArgs e)
    {
        _onReloadEnd.Raise(this, null);
    }

    void Update()
    {
        if (debugMode) DebugMouse();       
        weaponHandler.SetAim(Camera.main.transform.forward * 200.0f);
        if (InputManager.Instance.HandleFireInput().IsPressed()) weaponHandler.Shoot();
    }

    void DebugMouse()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
        else if (Input.GetMouseButtonDown(0)) Cursor.lockState = CursorLockMode.Locked;
    }
}
