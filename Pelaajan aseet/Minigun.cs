using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon {

    private Transform rotateObject;
    private float currentSpinSpeed;
    private float startShootingAt;
    private float spinStep;

    [SerializeField] private GameObject bulletHole;

    private GameObject player;

    private void Awake()
    {
        audio = this.GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        spinStep = 45f;
        startShootingAt = 55; // Mitä isompi luku niin sitä kauemmin ampumisen aloittaminen kestää.

        Initialize(this.transform); // Get all weapon components from weaponPrefab.

        rotateObject = Utility.FindTransformFromChildren("Rotate", this.transform);
    }

    float startingTime;

    new void Update()
    {
        startingTime += Time.deltaTime;

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

        rotateObject.transform.Rotate(new Vector3(0, 0, 1f * currentSpinSpeed));

        // Fire
        if (Input.GetButton("Fire1"))
        {
            if (weaponStats.currentRateOfFire <= 0)
            {
                    // Rotate weapon
                    if (startShootingAt >= currentSpinSpeed)
                        currentSpinSpeed += spinStep * Time.deltaTime;

                    if (currentSpinSpeed < startShootingAt)
                        return;

                if (AllowShoot())
                {

                    // Plqy shooting sound
                    if (!audio.isPlaying)
                        audio.Play();

                    shootingLight.light.intensity = shootingLight.lightIntensity;

                    weaponStats.currentRateOfFire = weaponStats.rateOfFire;

                    for (int i = 0; i < weaponStats.bulletsPerShot; i++)
                    {
                        float spread = weaponStats.bulletSpread;
                        Vector3 rPos = new Vector3(-Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
                        Vector3 defaultRotation = shootingPoint.transform.rotation.eulerAngles;
                        Vector3 finalRotation = defaultRotation + rPos;

                        SpawnBullet(finalRotation);
                        SpawnLightEffect(finalRotation);
                        //SpawnEmptyShell();
                    }

                    // Show particle effect
                    shootingLight.shootingParticle.Play();

                    // Play sound
                    PlayGunSound(true);

                    // Check if player is using scope and do scope wiggle.
                    scope.MoveScope(weaponStats.bulletSpread, 0f);
                }
                else
                {
                    PlayGunSound(false);
                }
            }
        }
        else
        {
            HeatLevel(Time.deltaTime);

            // Play shooting animation
            /*
            if (gunAnimationController.enabled)
                gunAnimationController.enabled = false;
            */

            scope.ResetScope(5f);

            if (currentSpinSpeed > 0)
                currentSpinSpeed -= currentSpinSpeed * Time.deltaTime;
            else if (currentSpinSpeed < 0)
                currentSpinSpeed = 0;

            PlayGunSound(false);
        }

        /*
        if (Input.GetButtonDown("Fire2"))
        {
            if (scope.enabled)
            {
                // weapon.scope.camera.depth = 1;
                if (startingTime > 0.5f)
                {
                    scope.animator.speed = 1;
                    scope.animator.Play("zoomIn");
                    scope.currentAnimationTime = 0;
                    gamemanager.GetComponent<GameManager>().crosshair.SetActive(false);
                }
            }

        }
        else if (Input.GetButtonUp("Fire2"))
        {
            scope.animator.Play("zoomOut", 0, scope.maxAnimationTime - scope.currentAnimationTime);
            scope.currentAnimationTime = 0f;

            // Reset scope zoom
            gunCamera.fieldOfView = gunGamera_defaultFieldOfView;
            scope.current_fieldOfView = Weapon.default_fieldOfView;
            gamemanager.GetComponent<GameManager>().crosshair.SetActive(true);

            //scope.scopeImage.SetActive(false);
            if (scope._scopeImage != null)
                Destroy(scope._scopeImage);

            weaponPrefab.SetActive(true);
            mainCamera.fieldOfView = scope.nornal_fieldOfView;

            gunCamera.cullingMask = (1 << LayerMask.NameToLayer("Gun") | 1 << LayerMask.NameToLayer("Shell")); // Guncamera --> cullingMask --> enable "Gun" layer and "Shell" layer.
        }

        if (Input.GetButton("Fire2"))
        {
            // Check if fully "loaded" scope
            if (scope.animator.GetCurrentAnimatorStateInfo(0).IsName("zoomIn") && scope.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                // zoom finished playing.
                if (weaponPrefab.activeSelf)
                    weaponPrefab.SetActive(false);

                if (scope.currentAnimationTime < scope.maxAnimationTime)
                    scope.currentAnimationTime += Time.deltaTime;
                else
                    scope.currentAnimationTime = scope.maxAnimationTime;

                if (scope._scopeImage == null)
                {
                    scope._scopeImage = Instantiate(scope.scopeImage);
                    scope._scopeImage.transform.SetParent(canvas.transform, false);
                }

                gunCamera.cullingMask = (1 << LayerMask.NameToLayer("Gun")); // Guncamera --> cullingMask --> enable "Gun" layer.
            }

            scope.currentAnimationTime += Time.deltaTime;

            // Zoom in
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                // Scroll up
                if (scope.current_fieldOfView - scope.zoomStep > scope.maxZoom_fieldOfView)
                {
                    scope.current_fieldOfView -= scope.zoomStep;
                }
            }
            // Zoom out 
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                // Scroll down
                if (scope.current_fieldOfView < scope.nornal_fieldOfView + scope.zoomStep)
                {
                    scope.current_fieldOfView += scope.zoomStep;
                }
            }

            //weapon.scope.camera.fieldOfView = weapon.scope.current_fieldOfView;
            mainCamera.fieldOfView = scope.current_fieldOfView;
        }

        */

    }

}
