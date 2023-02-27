using Pelumi.Juicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : Weapon
{
    [SerializeField] private Transform thrower;
    [SerializeField] private JuicerVector3Properties juicerVector3Properties;

    public override void ShootProjectile(Vector3 targetPos)
    {
        base.ShootProjectile(targetPos);

        StartCoroutine(Juicer.DoVector3(null, Vector3.zero,  (rotation) => thrower.localEulerAngles = rotation, juicerVector3Properties, null));

        //for (int i = 0; i < firePoint.Length; i++)
        //{
        //    Projectile projectile = Instantiate(weaponSO.projectile, firePoint[i].position, Quaternion.identity);
        //    StartCoroutine(Juicer.DoMove(projectile.transform, firePoint[i].position, targetPos, 45));
        //    ModifyAmmo(currentAmmo - 1);
        //}
    }
}
