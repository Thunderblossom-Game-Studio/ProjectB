using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiCannon : Weapon
{
    public override void ShootProjectile(Vector3 targetPos, Action OnFireSuccess = null)
    {
        base.ShootProjectile(targetPos, OnFireSuccess);
        ServerShoot(targetPos);
    }

    [ServerRpc(RequireOwnership = false)]

    private void ServerShoot(Vector3 targetPos)
    {
        for (int i = 0; i < firePoint.Length; i++)
        {
            Vector3 aimDirection = (targetPos - firePoint[i].position).normalized;
            SpawnProjectile(firePoint[i].position, aimDirection);
            ModifyAmmo(currentAmmo - 1);
        }
    }

    [ServerRpc]
    public void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        GameObject projectile = Instantiate(weaponSO.projectile, position, Quaternion.LookRotation(direction, Vector3.up)).gameObject;
        ServerManager.Spawn(projectile);
        SetSpawnedProjectile(projectile);
    }

    [ObserversRpc]
    public void SetSpawnedProjectile(GameObject projectile)
    {
        projectile.GetComponent<DartProjectile>().SetUp(weaponSO.projectileSpeed);
    }
}
