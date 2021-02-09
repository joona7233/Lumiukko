using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : MonoBehaviour
{
    // Global settings for all Weapon's
    public static float default_fieldOfView = 60; // Defualt camera fieldOfView

    public string weaponName;
    [SerializeField] private bool isReloading;
    private float currentReloadStartTime;

    [Header("Aseen tilastot")]
    [SerializeField] protected WeaponStats weaponStats;

    [Header("Aseen osat")]
    public GameObject weaponPrefab;
    public GameObject bulletPrefab;
    public GameObject bulletShell;
    public GameObject bulletLight;

    [Header("")]

    public ShootingLight shootingLight;
    public Scope scope;

    [Header("Particle System")]
    //public ParticleSystem gunFire;
    public ParticleSystem gunSmoke;
    [Range(0, 1)] public float heatLevel;

    [Header("Gun animator")]
    public Animator gunAnimationController;

    [Header("Other")]
    [HideInInspector] public Transform shootingPoint;
    [HideInInspector] public Transform shellPoint;
    [HideInInspector] public LineRenderer lr;
    [HideInInspector] public GameObject gamemanager;
    [HideInInspector] public GameObject canvas;
    [HideInInspector] public Camera gunCamera;
    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public LayerMask playermask;
    [HideInInspector] public LayerMask defaultmask;
    [HideInInspector] public float gunGamera_defaultFieldOfView;
    [HideInInspector] public AudioSource audio;

    public static Weapon Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize(Transform t)
    {
        shootingPoint = Utility.FindTransformFromChildren("ShootingPoint", t);
        shellPoint = Utility.FindTransformFromChildren("ShellPoint", t);
        lr = shootingPoint.GetComponentInChildren<LineRenderer>();
        gamemanager = GameObject.FindGameObjectWithTag("gamemanager");
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        gunCamera = GameObject.FindGameObjectWithTag("GunCamera").GetComponent<Camera>();
        mainCamera = Camera.main;
        weaponPrefab = Utility.FindTransformFromChildren("Weapon Parts", t).gameObject;
        shootingLight.light = Utility.FindTransformFromChildren("ShootingLight", t).GetComponent<Light>();
        shootingLight.shootingParticle = Utility.FindTransformFromChildren("ps-gunfire", t).GetComponent<ParticleSystem>();

        playermask = LayerMask.GetMask("Player");
        defaultmask = LayerMask.GetMask("Default");
        gunGamera_defaultFieldOfView = gunCamera.fieldOfView;
        scope.animator = gunCamera.GetComponent<Animator>();
        scope.scopeImage = gamemanager.GetComponent<GameManager>().scopeImage;

        gunAnimationController = t.GetComponent<Animator>();
        gunAnimationController.enabled = false;

        audio = t.GetComponent<AudioSource>();
        weaponStats.currentRateOfFire = 0;

        //scope.animator.Play("");
        //gunAnimationController.Play("zoomOut");


        // Lataa aseen tiedot GameManagerista
        weaponStats = gamemanager.GetComponent<GameManager>().GetWeaponStats(t.name);
    }

    protected void PlayGunSound(bool play)
    {
        if (play)
        {
            if (!audio.isPlaying)
                audio.Play();
        }
        else if (audio.isPlaying)
            audio.Stop();
    }

    protected void Update()
    {
        if (!Input.GetButton("Fire1") || !AllowShoot(true))
        {
            if(gunAnimationController.enabled)
            {
                gunAnimationController.enabled = false;
            }
        }

        // Lataa ase pelaajan komennosta
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            isReloading = true;
            currentReloadStartTime = weaponStats.reloadTime;
        }
        
        // Lataa ase automaattisesti
        if(weaponStats.ammo.ammoInClip <= 0 && !isReloading)
        {
            isReloading = true;
            currentReloadStartTime = weaponStats.reloadTime;
        }

        // Laske lataus ajasta taaksepäin kun pelaaja on ladannut aseen, jotta aseen lataus onnistuu seuraavallakin kerralla.
        if (currentReloadStartTime > 0)
        {
            currentReloadStartTime -= Time.deltaTime;
        }
        else if (currentReloadStartTime < 0)
            currentReloadStartTime = 0;

        if (currentReloadStartTime <= 0 && isReloading)
        {
            ReloadWeapon();
        }
    }

    protected bool AllowShoot(bool onlyCheck = false)
    {
        if (CheckForAmmo(!onlyCheck))
            return true;

        return false;
    }

   public void UpdatePlayerUI()
    {
        GameManager.Instance.playerUI.currentAmmo.text = weaponStats.ammo.ammoInClip + " \\ " + weaponStats.ammo.currentAmmo;     
    }

    protected bool CheckForAmmo(bool decreaseBullets)
    {
        if(weaponStats.ammo.ammoInClip > 0)
        {
            if (isReloading)
                return false;

            if (decreaseBullets)
            {
                // Ammusten vähennys
                weaponStats.ammo.ammoInClip -= (1 * weaponStats.bulletsPerShot);

                if (weaponStats.ammo.ammoInClip < 0)
                    weaponStats.ammo.ammoInClip = 0;

                // Päivitä pelaajan UI
                UpdatePlayerUI();
            }
    
            return true;
        }

        return false;
    }

    protected void SpawnBullet(Vector3 finalRotation)
    {       
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.GetComponent<Bullet>().damage = weaponStats.damage;
        bullet.transform.position = shootingPoint.transform.position;
        bullet.transform.rotation = Quaternion.Euler(finalRotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weaponStats.bulletSpeed;
        Destroy(bullet, 5f);

        if(gunAnimationController != null && !gunAnimationController.enabled)
        {
            gunAnimationController.enabled = true;
        }

        Player.ammoUsed++;

        #region Object pooling (lagaa jostain syystä, sen takia en käytä.
        /*
        GameObject bullet = ObjectPooler.Instance.ReuseObject("Bullet");
        bullet.GetComponent<Bullet>().damage = weaponStats.damage;
        bullet.transform.parent = null;
        bullet.transform.position = shootingPoint.transform.position;
        bullet.transform.rotation = Quaternion.Euler(finalRotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weaponStats.bulletSpeed;   
        */
        #endregion
    }

    protected void SpawnLightEffect(Vector3 finalRotation)
    {
        // Instantiate light effect
        #region OLD
        
        GameObject lightEffect = Instantiate(bulletLight);
        //lightEffect.transform.parent = gamemanager.transform;
        lightEffect.transform.rotation = Quaternion.Euler(finalRotation);
        lightEffect.transform.position = shootingPoint.transform.position;
        lightEffect.GetComponent<Rigidbody>().velocity = lightEffect.transform.forward * weaponStats.bulletSpeed;
        Destroy(lightEffect, 5f);

        #endregion
        #region object pooling tapa.
        /*
        GameObject lightEffect = ObjectPooler.Instance.ReuseObject("BulletLight");
        lightEffect.transform.parent = null;
        lightEffect.transform.rotation = Quaternion.Euler(finalRotation);
        lightEffect.transform.position = shootingPoint.transform.position;
        lightEffect.GetComponent<Rigidbody>().velocity = lightEffect.transform.forward * weaponStats.bulletSpeed;
        */
        #endregion
    }

    protected void SpawnEmptyShell()
    {
        // Instantiate empty shell's     
        #region OLD
        
        GameObject shell = Instantiate(bulletShell);
        shell.transform.position = shellPoint.transform.position;
        shell.transform.rotation = shootingPoint.transform.rotation;
        //shell.transform.parent = gamemanager.transform;
        shell.GetComponent<Rigidbody>().velocity = shellPoint.transform.forward * 50.5f * Time.deltaTime;
        Destroy(shell, 2f);

        #endregion
        #region obj pool
        /*
        GameObject shell = ObjectPooler.Instance.ReuseObject("Shell");
        shell.transform.position = shellPoint.transform.position;
        shell.transform.rotation = shootingPoint.transform.rotation;
        shell.transform.parent = null;
        shell.GetComponent<Rigidbody>().velocity = shellPoint.transform.forward * 50.5f * Time.deltaTime;
        */
        #endregion
    }


    private void ReloadWeapon()
    {
        if(weaponStats.ammo.ammoInClip < weaponStats.ammo.clipSize)
        {
            /*
            if(weaponStats.ammo.currentAmmo >= weaponStats.ammo.clipSize)
            {
                // Anna täysi lipas pelaajalle
                weaponStats.ammo.currentAmmo -= (weaponStats.ammo.clipSize - weaponStats.ammo.ammoInClip);
                weaponStats.ammo.ammoInClip = weaponStats.ammo.clipSize;
            }
            else
            {
                // Lataa vajaa
                weaponStats.ammo.ammoInClip += weaponStats.ammo.currentAmmo;
                weaponStats.ammo.currentAmmo = 0;
            }
            */

            int ammoNeededToFillClip = weaponStats.ammo.clipSize - weaponStats.ammo.ammoInClip;
            weaponStats.ammo.currentAmmo -= ammoNeededToFillClip;
            weaponStats.ammo.ammoInClip = weaponStats.ammo.clipSize;

            // Pävitetään pelaajan UO
            UpdatePlayerUI();
        }

        isReloading = false;
    }

    public void HeatLevel(float increment)
    {
        if(increment > 0)
        {
            heatLevel += increment;
        }
        else if(increment < 0)
        {
            if (heatLevel > 0)
                heatLevel -= increment;
            else if (heatLevel < 0)
                heatLevel = 0;
        }   
    }

    public static Weapon GiveWeapon(List<Weapon> ListContainingAllWeapons, string weaponName)
    {
        for(int i=0; i < ListContainingAllWeapons.Count; i++)
        {
            if(ListContainingAllWeapons[i].weaponName == weaponName)
            {
                return ListContainingAllWeapons[i];
            }
        }

        MonoBehaviour.print("Error cannot find weapon: " + weaponName);
        return null;
    }

    public static List<Weapon> InitializeWeapon(List<Weapon> list)
    {
        List<Weapon> w = new List<Weapon>();
        w = list;
        for(int i=0; i < list.Count; i++)
        {
            if(list[i].weaponPrefab != null)
            {
                w[i].weaponName = list[i].weaponPrefab.name; // set weapon name

                //w[i].gunFire = Find(list[i].weaponPrefab.GetComponentsInChildren<Transform>(), "ps-gunfire").GetComponent<ParticleSystem>(); // gunfire particle system

                w[i].shootingLight.light = Find(list[i].weaponPrefab.GetComponentsInChildren<Transform>(), "shootingLight").GetComponent<Light>(); // find light

                w[i].shootingLight.shootingParticle = Find(list[i].weaponPrefab.GetComponentsInChildren<Transform>(), "ps-gunfire").GetComponent<ParticleSystem>();
            }
        }

        return w;
    }

    private static Transform Find(Transform[] t, string name)
    {
        foreach(Transform _t in t)
        {
            if (_t.name == name)
                return _t;
        }

        return null;
    }


}
