using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCannon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] private Transform shootCam;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform ammoParent;
    [SerializeField] private GameObject ammo;
    //[SerializeField] private GameObject waterParticleEffect;
    [SerializeField] private float projectileHitMissDistance = 25f;
    private float timeSinceLastFired;

    private void Start()
    {
        weaponData.maxWeaponAmmo = 100;
        weaponData.currentWeaponAmmo = weaponData.maxWeaponAmmo;
    }

    private void Update()
    {
        timeSinceLastFired += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }


        //Cooldown mechanic not finished, might go back to it later
        /*if()
        {
        
        }*/
    }

    private bool CanFire() => !weaponData.coolingDown && timeSinceLastFired > 0.1f / (weaponData.weaponFireRate / 60f);

    private void Fire()
    {
        if(weaponData.currentWeaponAmmo > 0) //if weapon ammo is greater than 0
        {
            if(CanFire())
            {
                RaycastHit hit;
                GameObject projectile = GameObject.Instantiate(ammo, shootPoint.position, Quaternion.identity, ammoParent);
                //GameObject particle = GameObject.Instantiate(waterParticleEffect, shootPoint.position, Quaternion.identity, ammoParent);
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
                weaponData.currentWeaponAmmo--;
                timeSinceLastFired = 0;
                //OnWeaponFired();
            }
            else
            {
                //Debug.Log("WEAPON IS COOLING DOWN");
            }
        }
        else //if ammo is 0 or less
        {
            Debug.Log("OUT OF AMMO!");
        }
    }

    /*public void StartingCooldown()
    {
        if(!weaponData.coolingDown)
        {
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        weaponData.coolingDown = true;
        yield return new WaitForSeconds(weaponData.weaponCooldownTime);
        weaponData.coolingDown = false;
    }*/

    /*private void OnWeaponFired()
    {
        //
    }*/
}
