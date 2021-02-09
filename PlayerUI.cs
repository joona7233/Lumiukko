using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerUI {

    public Text money;
    public Text currentWeapon;
    public Text currentAmmo;

    public Image healthbar;

    public Image energyBar;
    public void UpdateEnergyBar(float currentEnergy, float maxEnergy)
    {
        energyBar.fillAmount = (currentEnergy / maxEnergy);
    }

    public void UpdatePlayerUI(string money, string currentWeapon, string currentAmmo)
    {
        if(money != "")
            this.money.text = money;

        if(currentWeapon != "")
            this.currentWeapon.text = currentWeapon;

        if(currentAmmo != "")
            this.currentAmmo.text = currentAmmo;
    }
}
