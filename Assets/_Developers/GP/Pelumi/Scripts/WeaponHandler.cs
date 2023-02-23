using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHandler : MonoBehaviour
{
    public enum WeaponState { Idle, Firing, Reloading }
    
    [Header("Turret gameobjects")]
    [SerializeField] private Transform turretBase;
    [SerializeField] private Transform turretBarrel;

    [Header("Rotation settings")]
    [Range(0, 180)]
    [SerializeField] private float rightRotationLimit;
    [Range(0, 180)]
    [SerializeField] private float leftRotationLimit;
    [Range(0, 90)]
    [SerializeField] private float elevationRotationLimit;
    [Range(0, 90)]
    [SerializeField] private float depressionRotationLimit;
    [Range(0, 300)]
    [SerializeField] private float baseTurnSpeed;
    [Range(0, 300)]
    [SerializeField] private float turrentTurnSpeed;

    [Header("Shooting")]
    [SerializeField] private LayerMask detectMask;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private Transform firePoint;

    [Viewable] [SerializeField] private int currentAmmo;
    [Viewable] [SerializeField] private WeaponState weaponState;
    [Viewable] [SerializeField] private float timer = 0;

    public event EventHandler OnAmmoChanged;
    public event EventHandler OnReloadStart;
    public event EventHandler<float> OnReloading;
    public event EventHandler OnReloadEnd;

    private Vector3 aimPoint;
    public void SetAim(Vector3 AimPosition) => aimPoint = AimPosition;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => weaponSO.maxAmmo;

    private void Start()
    {
        ModifyAmmo(weaponSO.maxAmmo);
    }
    
    private void Update()
    {
        HandleHorizontalRotation();
        HandleVerticalRotation();
    }

    private void HandleHorizontalRotation()
    {
        Vector3 targetPositionInLocalSpace = aimPoint;
        targetPositionInLocalSpace.y = 0.0f;
        Vector3 limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, (targetPositionInLocalSpace.x >= 0.0f) ?  Mathf.Deg2Rad * rightRotationLimit : Mathf.Deg2Rad * leftRotationLimit, float.MaxValue);
        Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);    
        turretBase.rotation = Quaternion.RotateTowards(turretBase.rotation, whereToRotate, baseTurnSpeed * Time.deltaTime);
    }

    private void HandleVerticalRotation()
    {
        Vector3 targetPositionInLocalSpace = aimPoint;
        targetPositionInLocalSpace.x = 0.0f;
        Vector3 limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, (targetPositionInLocalSpace.y >= 0.0f) ? Mathf.Deg2Rad * elevationRotationLimit : Mathf.Deg2Rad * depressionRotationLimit, float.MaxValue);
        Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);
        turretBarrel.localRotation = Quaternion.RotateTowards(turretBarrel.localRotation, whereToRotate, turrentTurnSpeed * Time.deltaTime);
    }

    public void Shoot()
    {
        if (weaponState == WeaponState.Reloading) return;
        
        if (timer <= 0)
        {
            TryShootProjectile();
            timer = weaponSO.fireRate;
        }
        else timer -= Time.deltaTime;
    }
    
    public void TryShootProjectile()
    {
        if (currentAmmo > 0) ShootProjectile(); else if (weaponState != WeaponState.Reloading) StartCoroutine(Reload());
    }

    public void ShootProjectile()
    {
        weaponState = WeaponState.Firing;
        Vector3 aimDirection = (GetWorldMousePosition() - firePoint.position).normalized;
        Projectile projectile = Instantiate(weaponSO.projectile, firePoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        ModifyAmmo(currentAmmo - 1);
    }

    public void ModifyAmmo(int newValue)
    {
        currentAmmo = newValue;
        OnAmmoChanged?.Invoke(this, EventArgs.Empty);
    }

    public IEnumerator Reload()
    {
        OnReloadStart?.Invoke(this, EventArgs.Empty);
        float reloadTime = weaponSO.reloadTime;
        weaponState = WeaponState.Reloading;
        while(reloadTime > 0)
        {
            reloadTime -= Time.deltaTime;
            OnReloading?.Invoke(this, reloadTime/ weaponSO.reloadTime);
            yield return null;
        }
        ModifyAmmo(weaponSO.maxAmmo);
        weaponState = WeaponState.Idle;
        OnReloadEnd?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetWorldMousePosition()
    {
        Vector3 worldMousePosition = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, detectMask)) worldMousePosition = hit.point; else worldMousePosition = ray.GetPoint(200.0f); return worldMousePosition;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (turretBarrel != null) Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.forward * 200.0f);
    }
}
