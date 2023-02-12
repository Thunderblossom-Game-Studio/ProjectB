using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCannon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] private Transform shootCam;
    [SerializeField] private GameObject ammo;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform ammoParent;
    [SerializeField] private float projectileHitMissDistance = 25f;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        RaycastHit hit;
        GameObject projectile = GameObject.Instantiate(ammo, shootPoint.position, Quaternion.identity, ammoParent);
        WaterAmmo waterAmmo = projectile.GetComponent<WaterAmmo>();
        if (Physics.Raycast(shootCam.position, shootCam.forward, out hit, Mathf.Infinity))
        {
            waterAmmo.target = hit.point;
            waterAmmo.hit = true;
        }
        else
        {
            waterAmmo.target = shootCam.position + shootCam.forward * projectileHitMissDistance;
            waterAmmo.hit = false;
        }
    }
}
