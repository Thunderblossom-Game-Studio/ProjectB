using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int currentWeaponAmmo;
    public int maxWeaponAmmo;
    public float weaponRange;
    public float weaponFireRate; //"Water" rate!
    //[SerializeField] private float weaponCooldownTime;
    [HideInInspector]
    public bool coolingDown;
}
