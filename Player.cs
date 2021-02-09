using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static int money;
    public static int score;
    public static bool immunity;

    public new static string name;
    public static int ammoUsed;
    public static int ammoHit;
    public static int enemiesKilled;

    public static int maxHealth;
    public static int currentHealth;

    public static bool alive;

    /*
    public int money;
    public int score;

    public int maxHealth;
    public int currentHealth;

    public bool alive;
    */


    //[SerializeField] private List<GameObject> weapons = new List<GameObject>();

    [SerializeField] private bool givePlayerAllWeapons;
    public List<WeaponPrefab> weapons = new List<WeaponPrefab>();

    private int currentWeapon;
    private int maxWeapon;

    //public static bool alive { public get; private set; }

    private GameObject gamemanager;
    private Transform weaponParent;

    private Camera gunCamera;

    public static Player Instance;

    private void Awake()
    {
        Instance = this;
        gamemanager = GameObject.FindGameObjectWithTag("gamemanager");
        weaponParent = GameObject.FindGameObjectWithTag("WeaponParent").transform;
        gunCamera = GameObject.FindGameObjectWithTag("GunCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        immunity = false;

        Money(0);
        maxHealth = 1000;
        currentHealth = maxHealth;

        alive = true;

        // Give player all weapons
        if(givePlayerAllWeapons)
            weapons = GameManager.Instance.weapons;
        else
        {
            LoadWeapons();
        }

        //AddWeapon(null);

        // Layer's
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Shell"), true);
    }

    public void LoadWeapons()
    {
        // Give only "owned" weapons
        for (int i = 0; i < GameManager.Instance.weapons.Count; i++)
        {
            if (GameManager.Instance.weapons[i].owned)
            {
                //weapons.Add(GameManager.Instance.weapons[i]);
                GameManager.Instance.weapons[i].weaponStats.ammo.FillAmmo();
                AddWeapon(GameManager.Instance.weapons[i]);
            }
        }

        currentWeapon = 0;
        ChangeWeapon();
    }

    public void Reward(int money, int score, bool countAsEnemyKill = true)
    {
        Player.money += money;
        Player.score += score;
        GameManager.Instance.playerUI.UpdatePlayerUI(Player.money + "$", "", "");

        if (countAsEnemyKill)
            enemiesKilled++;
    }

    public static void Damage(int amount)
    {
        if (immunity)
            return;

        currentHealth -= amount;
        GameManager.Instance.playerUI.healthbar.fillAmount = ((float)currentHealth / (float)maxHealth);

        if(currentHealth <= 0)
        {
            alive = false;
            GameManager.Instance.Gameover(true);
        }
    }

    public static void Money(int amount)
    {
        Player.money += amount;
        GameManager.Instance.playerUI.money.text = Player.money + "$";
    }

    public void AddWeapon(WeaponPrefab w)
    {
        if (w != null)
        {
            w.owned = true;
            if(!weapons.Contains(w))
                weapons.Add(w);
        }

        maxWeapon = weapons.Count - 1;
    }

    private void ChangeWeapon()
    {
        GameObject[] currentWeapons = GameObject.FindGameObjectsWithTag("Weapon");
        for(int i=0; i < currentWeapons.Length; i++)
        {
            Destroy(currentWeapons[i]);
        }

        GameObject weapon = Instantiate(weapons[currentWeapon].prefab, weaponParent);

        // Muuta peliobjektin nimeä, jotta voimme myöhemmin löytää tiedot GameManager scriptistä.
        weapon.name = weapon.name.Replace("(Clone)", "");
        weapon.name = weapon.name.Trim();

        weapon.transform.localRotation = Quaternion.Euler(weapons[currentWeapon].rotation);
        weapon.transform.localPosition = weapons[currentWeapon].position;
        weapon.tag = "Weapon";

        // gunCamera animaatio ei toimi aseenvaihdossa, jatka -->
        //if(Time.realtimeSinceStartup > 1)
        gunCamera.gameObject.GetComponent<Animator>().Play("zoomOut");

        GameObject scope = GameObject.FindGameObjectWithTag("Scope");
        if (scope != null)
            Destroy(scope);

        // Päivitä UI
        GameManager.Instance.playerUI.UpdatePlayerUI("", weapons[currentWeapon].weaponName, weapons[currentWeapon].weaponStats.ammo.ammoInClip + " \\ " + weapons[currentWeapon].weaponStats.ammo.currentAmmo);

        //print(currentWeapon + "/" + maxWeapon);
    }

    /*
    public void Damage(int amount)
    {
        currentHealth -= amount;
        
        if(currentHealth <= 0)
        {
            alive = false;
            currentHealth = 0;
        }

        GameManager.Instance.playerUI.healthbar.fillAmount = (currentHealth / maxHealth);
    }
    */

    private void Update()
    {
        //print(currentWeapon + "/" + maxWeapon);

        if (!Input.GetButton("Fire2") && !GameManager.Instance.ShopOpen())
        {
            // Change weapon
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                // Scroll up
                if (maxWeapon > currentWeapon)
                {
                    currentWeapon++;
                    ChangeWeapon();
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                // Scroll down
                if (currentWeapon > 0)
                {
                    currentWeapon--;
                    ChangeWeapon();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && maxWeapon >= 0)
        {
            currentWeapon = 0;
            ChangeWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && maxWeapon >= 1)
        {
            currentWeapon = 1;
            ChangeWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && maxWeapon >= 2)
        {
            currentWeapon = 2;
            ChangeWeapon();
        }


        // Avaa kartta
        if (Input.GetKey(KeyCode.Tab) && alive)
        {
            GameManager.Instance.Map(true);
        }
        else
        {
            GameManager.Instance.Map(false);
        }

        // Avaa kauppa / Sulje kauppa
        if (Input.GetKeyDown(KeyCode.Escape) && alive)
        {
            GameManager.Instance.OpenShop();
        }


    }
}
