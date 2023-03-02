using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponSO : ScriptableObject
{
    public Projectile projectile;
    public float projectileSpeed;
    public float fireRate;
    public float reloadTime;
    public int maxAmmo;
}