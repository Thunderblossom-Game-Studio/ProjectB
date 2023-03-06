using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiCannon : Weapon
{
    public override void ShootProjectile(Vector3 targetPos, Action OnFireSuccess = null)
    {
        base.ShootProjectile(targetPos, OnFireSuccess);

        for (int i = 0; i < firePoint.Length; i++)
        {
            Vector3 aimDirection = (targetPos - firePoint[i].position).normalized;
            DartProjectile projectile = Instantiate(weaponSO.projectile, firePoint[i].position, Quaternion.LookRotation(aimDirection, Vector3.up)) as DartProjectile;
            if (projectile) projectile.SetUp(weaponSO.projectileSpeed);
            ModifyAmmo(currentAmmo - 1);
        }
    }
}
