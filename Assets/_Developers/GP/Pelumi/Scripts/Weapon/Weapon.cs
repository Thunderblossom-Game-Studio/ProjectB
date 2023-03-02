using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponState { Idle, Firing, Reloading }

    [Header("Content")]
    [SerializeField] protected Transform content;

    [Header("Rotation settings")]
    [SerializeField] protected bool rotateVertical;
    [Range(0, 180)]
    [SerializeField] protected float rightRotationLimit = 180f;
    [Range(0, 180)]
    [SerializeField] protected float leftRotationLimit = 180f;
    [Range(0, 300)]
    [SerializeField] protected float turnSpeed = 300f;

    [Header("Shooting")]
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected Transform[] firePoint;

    [Viewable] [SerializeField] protected int currentAmmo;
    [Viewable] [SerializeField] protected WeaponState weaponState;
    [Viewable] [SerializeField] protected float timer = 0;

    public event EventHandler OnAmmoChanged;
    public event EventHandler OnReloadStart;
    public event EventHandler<float> OnReloading;
    public event EventHandler OnReloadEnd;

    protected Vector3 aimPoint;
    public void SetAim(Vector3 AimPosition) => aimPoint = AimPosition;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => weaponSO.maxAmmo;

    protected virtual void Start()
    {
        ModifyAmmo(weaponSO.maxAmmo);
    }

    protected virtual void Update()
    {
        HandleHorizontalAndVerticalRotation();
    }

    private void HandleHorizontalAndVerticalRotation()
    {
        Vector3 targetPositionInLocalSpace = aimPoint;
        if (!rotateVertical) targetPositionInLocalSpace.y = 0;
        Vector3 limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, (targetPositionInLocalSpace.x >= 0.0f) ? Mathf.Deg2Rad * rightRotationLimit : Mathf.Deg2Rad * leftRotationLimit, float.MaxValue);
        Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);
        content.rotation = Quaternion.RotateTowards(content.rotation, whereToRotate, turnSpeed * Time.deltaTime);
    }

    public void Shoot(Vector3 targetPos)
    {
        if (weaponState == WeaponState.Reloading) return;

        if (timer <= 0)
        {
            TryShootProjectile(targetPos);
            timer = weaponSO.fireRate;
        }
        else timer -= Time.deltaTime;
    }

    public void TryShootProjectile(Vector3 targetPos)
    {
        if (currentAmmo > 0) ShootProjectile(targetPos); else if (weaponState != WeaponState.Reloading) Reload();
    }

    public virtual void ShootProjectile(Vector3 targetPos)
    {
        weaponState = WeaponState.Firing;
    }

    public void ModifyAmmo(int newValue)
    {
        currentAmmo = newValue;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);

        if(currentAmmo <= 0) Reload();
    }

    public void Reload()
    {
        if (weaponState == WeaponState.Reloading) return;
        weaponState = WeaponState.Reloading;
        StartCoroutine(ReloadRoutine());
    }

    public IEnumerator ReloadRoutine()
    {
        OnReloadStart?.Invoke(this, EventArgs.Empty);
        float reloadTime = weaponSO.reloadTime;
        weaponState = WeaponState.Reloading;
        while (reloadTime > 0)
        {
            reloadTime -= Time.deltaTime;
            OnReloading?.Invoke(this, reloadTime / weaponSO.reloadTime);
            yield return null;
        }
        ModifyAmmo(weaponSO.maxAmmo);
        weaponState = WeaponState.Idle;
        OnReloadEnd?.Invoke(this, EventArgs.Empty);
    }
}
