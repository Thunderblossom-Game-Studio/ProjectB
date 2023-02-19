using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    [SerializeField] GunInfo ScriptableObject;
    [SerializeField] private Transform shootCam;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform ammoParent;
    [SerializeField] private float projectileHitMissDistance = 25f;
    private float timeSinceLastFired;
    private bool outOfMags;
    private int SelectFiringMode;

    public int pelletCount;
    public float spreadAngle;
    public float pelletFireVel = 1;
    List<Quaternion> pellets;


    private void Start()
    {
        ScriptableObject.gunReloading = false;
        ScriptableObject.gunCurrentAmmo = ScriptableObject.gunMaxAmmo;
        ScriptableObject.gunCurrentMagSize = ScriptableObject.gunMaxMagSize;
        outOfMags = false;
        SelectFiringMode = 1;

        pellets = new List<Quaternion>(pelletCount);
        for(int i = 0; i < pelletCount; i++)
        {
            pellets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    private void Update()
    {
        timeSinceLastFired += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F)) //Change firing mode
        {
            if(SelectFiringMode == 1)
            {
                SelectFiringMode = 2;
                Debug.Log("Semi-Auto equipped");
            }
            else if(SelectFiringMode == 2)
            {
                SelectFiringMode = 3;
                Debug.Log("Shotgun equipped");
            }
            else if(SelectFiringMode == 3)
            {
                SelectFiringMode = 1;
                Debug.Log("Fully Auto equipped");
            }
        }

        //Fully Auto
        if (SelectFiringMode == 1)
        {
            if (Input.GetMouseButton(0))
            {
                Fire();
            }
        }
        //Semi-Auto
        if (SelectFiringMode == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }
        //Shotgun
        if (SelectFiringMode == 3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FireShotgun();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }
        else if ((Input.GetKeyDown(KeyCode.R)) && (ScriptableObject.gunCurrentMagSize == 0))
        {
            Debug.Log("OUT OF RESERVE AMMO");
        }

        //Auto reload after ammo in "magazine" runs out
        if (ScriptableObject.gunCurrentAmmo == 0)
        {
            StartReload();
        }
    }

    //Reloading
    public void StartReload()
    {
        if (!ScriptableObject.gunReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        if (ScriptableObject.gunCurrentMagSize > 0)
        {
            Debug.Log("RELOADING...");
            ScriptableObject.gunReloading = true;
            yield return new WaitForSeconds(ScriptableObject.gunReloadTime);

            ScriptableObject.gunCurrentAmmo = ScriptableObject.gunMaxAmmo;
            ScriptableObject.gunCurrentMagSize--;

            ScriptableObject.gunReloading = false;
            Debug.Log("FINISHED RELOADING");
        }
        else if (!outOfMags)
        {
            Debug.Log("OUT OF RESERVE AMMO");
            outOfMags = true;
        }
    }

    private bool CanFire() => !ScriptableObject.gunReloading && timeSinceLastFired > 1f / (ScriptableObject.gunFireRate / 60f);

    private void Fire()
    {
        if (ScriptableObject.gunCurrentAmmo > 0)
        {
            if (CanFire())
            {
                Debug.Log("Fire!");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                GameObject projectile = GameObject.Instantiate(ScriptableObject.projectileType, shootPoint.position, Quaternion.identity, ammoParent);

                GunAmmo gunAmmo = projectile.GetComponent<GunAmmo>();
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    gunAmmo.target = hit.point;
                    gunAmmo.hit = true;
                }
                else
                {
                    gunAmmo.target = shootCam.position + shootCam.forward * projectileHitMissDistance;
                    gunAmmo.hit = false;
                }
                ScriptableObject.gunCurrentAmmo--;
                timeSinceLastFired = 0;
            }
        }
        else if (!ScriptableObject.gunReloading)//if ammo is 0 or less
        {
            Debug.Log("OUT OF AMMO!");
        }
        else if (ScriptableObject.gunReloading)//if ammo is 0 or less
        {
            Debug.Log("GUN IS RELOADING!");
        }
    }

    //Not working
    private void FireShotgun()
    {
        if (ScriptableObject.gunCurrentAmmo > 0)
        {
            if (CanFire())
            {
                Debug.Log("Fire!");
                RaycastHit hit;
                for (int i = 0; i < pelletCount; i++)
                {
                    pellets[i] = Random.rotation;
                    GameObject pellet = GameObject.Instantiate(ScriptableObject.projectileType, shootPoint.position, shootPoint.rotation);
                    pellet.transform.rotation = Quaternion.RotateTowards(pellet.transform.rotation, pellets[i], spreadAngle);
                    pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.right * pelletFireVel);
                    i++;
                }
                
            }
        }
        else if (!ScriptableObject.gunReloading)//if ammo is 0 or less
        {
            Debug.Log("OUT OF AMMO!");
        }
        else if (ScriptableObject.gunReloading)//if ammo is 0 or less
        {
            Debug.Log("GUN IS RELOADING!");
        }
    }
}
