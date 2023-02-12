using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float weaponDamage;
    public float weaponRange;
    public float weaponFireRate; //"Water" rate!
    public float weaponCooldownTime;
    public int weaponAmmo;
    [HideInInspector]
    public bool coolingDown;
}
