
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiCannon : Weapon
{
    [SerializeField] private int bulletPerShot;
    [SerializeField] private float spreadAngle = 10f;

    public override void ShootProjectile(Vector3 targetPos, Action OnFireSuccess = null)
    {
        base.ShootProjectile(targetPos, OnFireSuccess);
        Shoot(targetPos);
    }
    private void Shoot(Vector3 targetPos)
    {
        for (int i = 0; i < bulletPerShot; i++)
        {
            Vector3 aimDirection = (targetPos - firePoint[0].position).normalized;

            float offsetAngle = UnityEngine.Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Quaternion rotation = Quaternion.AngleAxis(offsetAngle, Vector3.up);
            Vector3 spreadDirection = rotation * aimDirection;

            SpawnProjectile(firePoint[0].position, spreadDirection);
        }
        ModifyAmmo(currentAmmo - 1);
    }

    public void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        Projectile projectile = Instantiate(weaponSO.projectile, position, Quaternion.LookRotation(direction, Vector3.up));
        (projectile as DartProjectile).SetUp(weaponSO.projectileSpeed, weaponSO.projectileDamage);
    }
}
