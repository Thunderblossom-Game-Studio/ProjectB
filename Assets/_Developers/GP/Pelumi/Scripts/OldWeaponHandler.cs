using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OldWeaponHandler : MonoBehaviour
{
    public enum WeaponState { Idle, Firing, Reloading }
    
    [Header("Turret gameobjects")]
    [SerializeField] private Transform turretBase;
    [SerializeField] private Transform turretBarrel;

    [Header("Rotation settings")]

    [SerializeField] private bool rotateVertical;
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
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private LayerMask shootingLayer;
    [SerializeField] private ParticleSystem testParticle;
    [SerializeField] private Transform[] firePoint;

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

    private Vector3 hitPoint;

    private void Start()
    {
        ModifyAmmo(weaponSO.maxAmmo);
    }
    
    private void Update()
    {
        HandleHorizontalAndVerticalRotation();
    }

    private void HandleHorizontalAndVerticalRotation()
    {
        Vector3 targetPositionInLocalSpace = aimPoint;
        if(!rotateVertical) targetPositionInLocalSpace.y = 0;
         Vector3 limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, (targetPositionInLocalSpace.x >= 0.0f) ? Mathf.Deg2Rad * rightRotationLimit : Mathf.Deg2Rad * leftRotationLimit, float.MaxValue);
        Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);
        turretBase.rotation = Quaternion.RotateTowards(turretBase.rotation, whereToRotate, baseTurnSpeed * Time.deltaTime);
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
        if (currentAmmo > 0) ShootProjectile(targetPos); else if (weaponState != WeaponState.Reloading) StartCoroutine(Reload());

        ShootHitScan();
    }

    public void ShootProjectile(Vector3 targetPos)
    {
        weaponState = WeaponState.Firing;

        for (int i = 0; i < firePoint.Length; i++)
        {
            Vector3 aimDirection = (targetPos - firePoint[i].position).normalized;
            Projectile projectile = Instantiate(weaponSO.projectile, firePoint[i].position, Quaternion.LookRotation(aimDirection, Vector3.up));
            ModifyAmmo(currentAmmo - 1);
        }
    }

    public void ShootHitScan()
    {
        for (int i = 0; i < firePoint.Length; i++)
        {
            DetectHit();
        }
    }

    public Ray GetRay(Vector3 startPos, Vector3 direction)
    {
        return new Ray(startPos, direction);
    }

    public void DetectHit()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999, shootingLayer))
        {
            Instantiate(testParticle, raycastHit.point, Quaternion.identity);
        }
        else
        {
            // If we did not hit anything
          //  ShotBulletTrail(currentWeapon.weaponMuzzle, (Camera.main.transform.forward * currentWeapon.weaponSO.weaponRange));
        }
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < firePoint.Length; i++)
        {
        //    Gizmos.DrawLine(firePoint[i].position, firePoint[i].position + firePoint[i].forward * 200.0f);
        }

        Gizmos.DrawWireSphere(hitPoint, .5f);
    }

    #region OldCode
    private void HandleHorizontalRotation()
    {
        Vector3 targetPositionInLocalSpace = aimPoint;
        targetPositionInLocalSpace.y = 0.0f;
        Vector3 limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, (targetPositionInLocalSpace.x >= 0.0f) ? Mathf.Deg2Rad * rightRotationLimit : Mathf.Deg2Rad * leftRotationLimit, float.MaxValue);
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
    #endregion
}
