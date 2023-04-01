
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private List<Weapon> allWeapon;
    [SerializeField] private GameEvent _onOnAmmoChanged;
    [SerializeField] private GameEvent _onReloadStart;
    [SerializeField] private GameEvent _onReloading;
    [SerializeField] private GameEvent _onReloadEnd;
    [SerializeField] private LayerMask detectMask;
    [SerializeField] private Transform gunSocket;

    [Viewable] [SerializeField] private Weapon currentWeapon;

    private void OnEnable()
    {
        currentWeapon = allWeapon[0];
        SubscribeWeaponEvent();
    }

    private void WeaponHandler_OnAmmoChanged(object sender, System.EventArgs e)
    {
        _onOnAmmoChanged.Raise(this, new int[] { currentWeapon.CurrentAmmo, currentWeapon.MaxAmmo });
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

    public void SubscribeWeaponEvent()
    {
        currentWeapon.OnAmmoChanged += WeaponHandler_OnAmmoChanged;
        currentWeapon.OnReloadStart += WeaponHandler_OnReloadStart;
        currentWeapon.OnReloading += WeaponHandler_OnReloadDuration;
        currentWeapon.OnReloadEnd += WeaponHandler_OnReloadEnd;
    }

    public void UnSubscribeWeaponEvent()
    {
        currentWeapon.OnAmmoChanged += WeaponHandler_OnAmmoChanged;
        currentWeapon.OnReloadStart += WeaponHandler_OnReloadStart;
        currentWeapon.OnReloading += WeaponHandler_OnReloadDuration;
        currentWeapon.OnReloadEnd += WeaponHandler_OnReloadEnd;
    }

    void Update()
    {
        DebugSwitchWeapons();

        if (debugMode) DebugMouse();

        currentWeapon.SetAim(Camera.main.transform.forward * 200.0f);
        if (currentWeapon is Catapult catapult) catapult.DrawPath(GetTargetPos(currentWeapon.Range));
        if (InputManager.Instance.HandleFireInput().IsPressed()) currentWeapon.Shoot(GetTargetPos(currentWeapon.Range), OnFireSuccess);
    }

    public void DebugSwitchWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeapon.transform.GetChild(0).gameObject.SetActive(false);
            UnSubscribeWeaponEvent();
            currentWeapon = allWeapon[ allWeapon.IndexOf(currentWeapon)  == allWeapon.Count  - 1 ? 0 : allWeapon.IndexOf(currentWeapon) + 1];
            currentWeapon.transform.GetChild(0).gameObject.SetActive(true);
            SubscribeWeaponEvent();
        }
    }

    public void OnFireSuccess()
    {
        AudioManager.PlaySoundEffect(currentWeapon.FireSoundID, true);
    }

    void DebugMouse()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;  else if (Input.GetMouseButtonDown(0)) Cursor.lockState = CursorLockMode.Locked;
    }

    public Vector3 GetTargetPos(float range)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (Physics.Linecast(Camera.main.transform.position, gunSocket.position, detectMask))
        {
            Debug.DrawLine(Camera.main.transform.position, gunSocket.position, Color.green);
            return ray.GetPoint(range);
        } 
        else
        {
            if (Physics.Raycast(ray, out RaycastHit hit, range, detectMask))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.magenta);
                return hit.point;
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.magenta);
                return ray.GetPoint(range);
            }
        }
    }
}
