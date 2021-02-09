using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ammo {

    public int ammoInClip;
    public int clipSize;
    public int currentAmmo;
    public int maxAmmo;

    public int GetRemainingAmmoTillMax()
    {
        if ((maxAmmo + clipSize) -(currentAmmo + ammoInClip) <= 0)
        {
            return 0;
        }
        else
            return (maxAmmo + clipSize) - (currentAmmo + ammoInClip); 
    }

    public void FillAmmo()
    {
        ammoInClip = clipSize;
        currentAmmo = maxAmmo;
    }
}
