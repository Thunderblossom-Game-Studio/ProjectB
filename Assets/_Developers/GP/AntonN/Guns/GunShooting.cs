using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [SerializeField] GunInfo gunInfo;
    [SerializeField] private Transform shootCam;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform ammoParent;
    [SerializeField] private GameObject ammo;
    [SerializeField] private float projectileHitMissDistance = 25f;
    private float timeSinceLastFired;
    private bool outOfMags;

    private void Start()
    {
        gunInfo.gunReloading = false;
        gunInfo.gunCurrentAmmo = gunInfo.gunMaxAmmo;
        gunInfo.gunCurrentMagSize = gunInfo.gunMaxMagSize;
        outOfMags = false;
    }

    private void Update()
    {
        timeSinceLastFired += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }
        else if ((Input.GetKeyDown(KeyCode.R)) && (gunInfo.gunCurrentAmmo == 0))
        {
            Debug.Log("OUT OF RESERVE MAGAZINES");
        }

        //Auto reload after ammo in "magazine" runs out
        if (gunInfo.gunCurrentAmmo == 0)
        {
            StartReload();
        }
    }

    //Reloading
    public void StartReload()
    {
        if (!gunInfo.gunReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        if (gunInfo.gunCurrentMagSize > 0)
        {
            Debug.Log("RELOADING...");
            gunInfo.gunReloading = true;
            yield return new WaitForSeconds(gunInfo.gunReloadTime);

            gunInfo.gunCurrentAmmo = gunInfo.gunMaxAmmo;
            gunInfo.gunCurrentMagSize--;

            gunInfo.gunReloading = false;
            Debug.Log("FINISHED RELOADING");
        }
        else if (!outOfMags)
        {
            Debug.Log("OUT OF RESERVE MAGAZINES");
            outOfMags = true;
        }
    }

    private bool CanFire() => !gunInfo.gunReloading && timeSinceLastFired > 1f / (gunInfo.gunFireRate / 60f);

    private void Fire()
    {
        if(gunInfo.gunCurrentAmmo > 0)
        {
            if(CanFire())
            {
                Debug.Log("Fire!");
                RaycastHit hit;
                GameObject projectile = GameObject.Instantiate(ammo, shootPoint.position, Quaternion.identity, ammoParent);

                GunAmmo gunAmmo = projectile.GetComponent<GunAmmo>();
                if (Physics.Raycast(shootCam.position, shootCam.forward, out hit, Mathf.Infinity))
                {
                    gunAmmo.target = hit.point;
                    gunAmmo.hit = true;
                }
                else
                {
                    gunAmmo.target = shootCam.position + shootCam.forward * projectileHitMissDistance;
                    gunAmmo.hit = false;
                }
                gunInfo.gunCurrentAmmo--;
                timeSinceLastFired = 0;
            }
        }
        else if(!gunInfo.gunReloading)//if ammo is 0 or less
        {
            Debug.Log("OUT OF AMMO!");
        }
        else if (gunInfo.gunReloading)//if ammo is 0 or less
        {
            Debug.Log("GUN IS RELOADING!");
        }
    }

    

}
