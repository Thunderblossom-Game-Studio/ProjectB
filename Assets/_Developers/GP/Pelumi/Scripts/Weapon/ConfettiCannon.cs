using FishNet;
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

    [ObserversRpc]
    private void ServerShoot(Vector3 targetPos)
    {
        for (int i = 0; i < firePoint.Length; i++)
        {
            Vector3 aimDirection = (targetPos - firePoint[i].position).normalized;
            SpawnProjectile(firePoint[i].position, aimDirection);
            ModifyAmmo(currentAmmo - 1);
        }
    }
    public void SpawnProjectile(Vector3 position, Vector3 direction)
    {
        Projectile go = Instantiate(weaponSO.projectile, position , Quaternion.LookRotation(direction, Vector3.up));
        InstanceFinder.ServerManager.Spawn(go.gameObject);
        go.GetComponent<DartProjectile>().SetUp(weaponSO.projectileSpeed);
    }
}
