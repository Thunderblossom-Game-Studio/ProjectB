using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("Projectile")]
    public Projectile projectile;
    public float projectileSpeed;
    public float projectileDamage;

    [Header("Stats")]
    public float range;
    public float fireRate;
    public float reloadTime;
    public int maxAmmo;
}