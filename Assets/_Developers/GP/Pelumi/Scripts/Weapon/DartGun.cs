
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartGun : Weapon
{
    public override void ShootProjectile(Vector3 targetPos, Action OnFireSuccess = null)
    {
        base.ShootProjectile(targetPos, OnFireSuccess);
        Shoot(targetPos);
    }

    private void Shoot(Vector3 targetPos)
    {
        Vector3 aimDirection = (targetPos - firePoint[0].position).normalized;
        SpawnProjectile(firePoint[0].position, aimDirection);
        ModifyAmmo(currentAmmo - 1);
    }
    public void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        Projectile projectile = Instantiate(weaponSO.projectile, position, Quaternion.LookRotation(direction, Vector3.up));
        (projectile as DartProjectile).SetUp(weaponSO.projectileSpeed);
    }
}
