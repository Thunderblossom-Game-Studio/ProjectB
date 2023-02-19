using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Gun", menuName="Gun")]
public class GunInfo : ScriptableObject
{
    public string gunName;
    public int gunMaxAmmo; //Maximum ammo the gun starts with and can have
    public int gunCurrentAmmo; //Ammo that the gun currently has
    public int gunMaxMagSize;
    public int gunCurrentMagSize;
    public float gunRange;
    public float gunFireRate;
    public GameObject projectileType;
    [HideInInspector] public bool gunReloading; //If the gun is in the process of reloading or not: true = reloading, false = not reloading
    public float gunReloadTime;
}
