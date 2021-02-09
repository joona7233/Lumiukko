using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponPrefab
{
    public string weaponName;
    public bool owned;

    public int price;
    public int ammo_price;

    [Header("")]

    public GameObject prefab;
    public RenderTexture shopImage;
    public WeaponStats weaponStats;

    //public Animator gunCameraAnimator;

    [Header("Adjustments")]
    public bool useCustomAdjustments;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public static WeaponPrefab LoadPrefabInfo(WeaponPrefab wp)
    {
        wp.position = wp.prefab.transform.localPosition;
        wp.rotation = new Vector3(wp.prefab.transform.localRotation.x, wp.prefab.transform.localRotation.y, wp.prefab.transform.localRotation.z);
        wp.scale = wp.prefab.transform.localScale;

        return wp;
    }

    public static WeaponPrefab GetWeaponPrefabByName(string name, List<WeaponPrefab> targetWeaponPrefab)
    {
        for(int i=0; i < targetWeaponPrefab.Count; i++)
        {
            if (targetWeaponPrefab[i].weaponName == name)
                return targetWeaponPrefab[i];
        }

        return null;
    }

}
