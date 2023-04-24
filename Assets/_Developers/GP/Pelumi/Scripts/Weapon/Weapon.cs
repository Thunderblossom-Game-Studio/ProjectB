
using JE.DamageSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponState { Idle, Firing, Reloading }

    [Header("Content")]
    public GameObject visual;
    [SerializeField] protected Transform content;

    [Header("Rotation settings")]
    [SerializeField] protected bool rotateVertical;
    [Range(0, 180)]
    [SerializeField] protected float rightRotationLimit = 180f;
    [Range(0, 180)]
    [SerializeField] protected float leftRotationLimit = 180f;
    [Range(0, 180)]
    [SerializeField] protected float upRotationLimit = 180f;
    [Range(0, 180)]
    [SerializeField] protected float downRotationLimit = 180f;
    [Range(0, 300)]
    [SerializeField] protected float turnSpeed = 300f;

    [Header("Shooting")]
    [SerializeField] protected string fireSoundID;
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected Transform[] firePoint;
    [SerializeField] protected GameObject hitEffect;
    [SerializeField] protected ParticleSystem fireParticle;

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
    public float Range => weaponSO.range;

    public WeaponState State => weaponState;
    public string FireSoundID =>fireSoundID;

    protected virtual void Start()
    {
        ModifyAmmo(weaponSO.maxAmmo);
    }

    protected virtual void Update()
    {
        NewHandleHorizontalAndVerticalRotation();

        if (timer < weaponSO.fireRate) timer += Time.deltaTime;
    }

    private void NewHandleHorizontalAndVerticalRotation()
    {
        Vector3 targetPositionInLocalSpace = aimPoint;
        targetPositionInLocalSpace.y = rotateVertical ? Mathf.Clamp(targetPositionInLocalSpace.y, -upRotationLimit, downRotationLimit) : 0;
        targetPositionInLocalSpace.x = Mathf.Clamp(targetPositionInLocalSpace.x, -leftRotationLimit, rightRotationLimit);
        Vector3 limitedRotation = Vector3.RotateTowards(Vector3.forward, targetPositionInLocalSpace, float.MaxValue, float.MaxValue);
        if (limitedRotation.magnitude < Mathf.Epsilon) return;
        Quaternion whereToRotate = Quaternion.LookRotation(limitedRotation);
        content.rotation = Quaternion.RotateTowards(content.rotation, whereToRotate, turnSpeed * Time.deltaTime);
    }

    public void Shoot(Vector3 targetPos, Action onFireSuccess = null)
    {
        if (timer < weaponSO.fireRate || weaponState == WeaponState.Reloading) return;
        TryShootProjectile(targetPos, onFireSuccess);
    }

    public void TryShootProjectile(Vector3 targetPos, Action onFireSuccess = null)
    {
        if (currentAmmo > 0)
        {
            ShootProjectile(targetPos, onFireSuccess);
        }
        else if (weaponState != WeaponState.Reloading) Reload();
    }

    public virtual void ShootProjectile(Vector3 targetPos, Action onFireSuccess = null)
    {
        weaponState = WeaponState.Firing;
        onFireSuccess?.Invoke();
        timer = 0;
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

    protected void DealDamage(GameObject damageObject, Vector3 targetPos)
    {
        if (!damageObject.TryGetComponent(out IDamageable damageAble))
        {
            if (!damageObject.transform.root.TryGetComponent(out damageAble))
            {
                Instantiate(hitEffect, targetPos, Quaternion.identity);
                return;
            }
        }
        Instantiate(hitEffect, targetPos, Quaternion.identity);
        damageAble.ReduceHealth(weaponSO.projectileDamage);
    }
}
