using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class WeaponStats {

    public Ammo ammo;
    public int damage;
    public float bulletSpread;
    public float bulletSpeed;
    public float rateOfFire;
    public int bulletsPerShot;
    [HideInInspector] public float currentRateOfFire;

    public float reloadTime;
    //public float currentReloadTime;
    //public bool isReloading;

    public bool laserSight;
}
