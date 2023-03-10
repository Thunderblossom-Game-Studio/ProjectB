using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

AudioManager.GetSoundEffectClip("NerfGunShot");
public class DartGun : Weapon
{
    public override void ShootProjectile(Vector3 targetPos, Action OnFireSuccess = null)
    {
        base.ShootProjectile(targetPos, OnFireSuccess);
        AudioManager.PlaySoundEffect("NerfGunShot");

        for (int i = 0; i < firePoint.Length; i++)
        {
            Vector3 aimDirection = (targetPos - firePoint[i].position).normalized;
            DartProjectile projectile = Instantiate(weaponSO.projectile, firePoint[i].position, Quaternion.LookRotation(aimDirection, Vector3.up)) as DartProjectile;
            if (projectile) projectile.SetUp(weaponSO.projectileSpeed);
            ModifyAmmo(currentAmmo - 1);
        }
    }
}
