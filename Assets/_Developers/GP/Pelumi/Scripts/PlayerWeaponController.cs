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

    [SerializeField] private LayerMask detectMask;
    [SerializeField] private Weapon weapon;

    private void OnEnable()
    {
        weapon.OnAmmoChanged += WeaponHandler_OnAmmoChanged;
        weapon.OnReloadStart += WeaponHandler_OnReloadStart;
        weapon.OnReloading += WeaponHandler_OnReloadDuration;
        weapon.OnReloadEnd += WeaponHandler_OnReloadEnd;
    }

    private void WeaponHandler_OnAmmoChanged(object sender, System.EventArgs e)
    {
        _onOnAmmoChanged.Raise(this, new int[] { weapon.CurrentAmmo, weapon.MaxAmmo });
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
        weapon.SetAim(Camera.main.transform.forward * 200.0f);
        if (InputManager.Instance.HandleFireInput().IsPressed()) weapon.Shoot(GetWorldMousePosition());
    }

    void DebugMouse()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
        else if (Input.GetMouseButtonDown(0)) Cursor.lockState = CursorLockMode.Locked;
    }

    public Vector3 GetWorldMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, detectMask)) return hit.point; else return ray.GetPoint(300.0f);
    }
}
