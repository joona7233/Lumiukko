using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapon {

    float startingTime;

    // Use this for initialization
    void Start () {
        Initialize(this.transform);
    }
	
	// Update is called once per frame
	new void Update () {
        startingTime = Time.realtimeSinceStartup;

        //if (gamemanager.GetComponent<GameManager>().pause)
        if (GameManager.pause)
            return;

        base.Update();

        // Reset currentRateOfFire
        if (weaponStats.currentRateOfFire > 0)
            weaponStats.currentRateOfFire -= Time.deltaTime;
        else if (weaponStats.currentRateOfFire < 0)
            weaponStats.currentRateOfFire = 0;

        // Reset shooting light
        if (shootingLight.currentIntensity > 0)
            shootingLight.currentIntensity = 0;

        // Apply light new value to Light component
        shootingLight.light.intensity = shootingLight.currentIntensity;

        // Fire
        if (Input.GetButton("Fire1"))
        {
            if (weaponStats.currentRateOfFire <= 0)
            {
                if (AllowShoot())
                {
                    shootingLight.light.intensity = shootingLight.lightIntensity;

                    weaponStats.currentRateOfFire = weaponStats.rateOfFire;

                    // Play shooting animation
                    if (!gunAnimationController.enabled)
                    {
                        gunAnimationController.enabled = true;
                        gunAnimationController.Play("shoot");
                    }

                    for (int i = 0; i < weaponStats.bulletsPerShot; i++)
                    {
                        float spread = weaponStats.bulletSpread;
                        Vector3 rPos = new Vector3(-Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
                        Vector3 defaultRotation = shootingPoint.transform.rotation.eulerAngles;
                        Vector3 finalRotation = defaultRotation + rPos;

                        SpawnBullet(finalRotation);
                        SpawnLightEffect(finalRotation);
                        SpawnEmptyShell();
                    }

                    // Show particle effect
                    shootingLight.shootingParticle.Play();

                    // Play sound
                    //PlayGunSound();

                    // Check if player is using scope and do scope wiggle.
                    scope.MoveScope(weaponStats.bulletSpread, 0f);
                }
                else
                {
                    if (gunAnimationController.enabled)
                        gunAnimationController.enabled = false;
                }
            }
        }
        else
        {
            
            //HeatLevel(Time.deltaTime);

            // Play shooting animation
            if (gunAnimationController.enabled)
                gunAnimationController.enabled = false;

            //scope.ResetScope(5f);
        }
    }
}
