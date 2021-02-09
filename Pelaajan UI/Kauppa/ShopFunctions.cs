using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Slider))]
public class ShopFunctions : MonoBehaviour {

    private Text weaponName;
    private Text sliderText;
    private Slider slider;
    private Button buy;

    private WeaponPrefab w = new WeaponPrefab();

    private Text priceText;

    private void Start()
    {
        if(this.GetComponentInParent<AlreadyBought>() != null)
        {
            weaponName = this.GetComponentInParent<AlreadyBought>().weaponName;
            sliderText = this.GetComponentInParent<AlreadyBought>().sliderText;
            slider = this.GetComponentInParent<AlreadyBought>().slider;
            buy = this.GetComponentInParent<AlreadyBought>().buy;

            OnStart();
        }
        else
        {
            weaponName = this.GetComponentInParent<ShopItem>().weaponName;
            priceText = this.GetComponentInParent<ShopItem>().priceText;
        }
    }

    public void OnStart()
    {
        w = WeaponPrefab.GetWeaponPrefabByName(weaponName.text, GameManager.Instance.weapons);
        slider.value = 0;
        slider.maxValue = w.weaponStats.ammo.GetRemainingAmmoTillMax();
        buy.GetComponentInChildren<Text>().text = "Valitse määrä";
    }

    public void PercentButton()
    {
        switch (this.GetComponentInChildren<Text>().text.Trim().ToLower())
        {
            case "10%":
                slider.value = (0.1f * (slider.maxValue));
                break;
            case "50%":
                slider.value = (0.5f * slider.maxValue);
                break;
            case "full":
                slider.value = (1f * (slider.maxValue));
                break;
        }

        if (slider.value * w.ammo_price > Player.money)
            slider.value = (Player.money / w.ammo_price);
    }

    public void Buy()
    {
        int price = this.GetComponentInParent<AlreadyBought>().totalPrice;
        if (Player.money >= price)
        {
            Player.Money(-price);
            w.weaponStats.ammo.currentAmmo += (int)slider.value;
            Weapon.Instance.UpdatePlayerUI();
            OnStart();
            GameManager.Instance.playerUI.currentAmmo.text = w.weaponStats.ammo.ammoInClip + " \\ " + w.weaponStats.ammo.currentAmmo;
        }

        Shop.Instance.UpdateShopMoney();
    }

    public void BuyWeapon()
    {
        int price;

        if(int.TryParse(priceText.text.Trim(), out price))
        {
            if (Player.money > price)
            {
                WeaponPrefab _w = WeaponPrefab.GetWeaponPrefabByName(weaponName.text.Trim(), GameManager.Instance.weapons);
                Player.Money(-price);

                // Laita aseen omistus arvo "true" ja lisää ase pelaajan ase listaan.
                Player.Instance.AddWeapon(_w);

                print(weaponName.text);

                Shop.Instance.LoadWeapons();
            }
        }

        Shop.Instance.UpdateShopMoney();
    }

    public void OnSliderChange()
    {
        string value = slider.value.ToString("f0");
        if (value == "")
            value = "0";

        sliderText.text = value;

        // Update price
        int price = (int)slider.value * w.ammo_price;
        buy.GetComponentInChildren<Text>().text = "Osta (" + price + ")";
        this.GetComponentInParent<AlreadyBought>().totalPrice = price;
    }
}
