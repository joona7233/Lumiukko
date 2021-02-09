using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static bool pause;

    [SerializeField] private GameObject map;
    private GameObject _map;

    [SerializeField] private Text fpsText;

    public GameObject crosshair;
    public GameObject hitUI;
    public GameObject hitSplash;
    public GameObject scoreSplash;

    [SerializeField] private GameObject shopUI;
    private GameObject _shopUI;

    [SerializeField] private GameObject pausemenu;
    private GameObject _pausemenu;

    [SerializeField] private GameObject highscoreUI;
    private GameObject _highscoreUI;

    [SerializeField] private GameObject gameoverUI;
    private GameObject _gameoverUI;

    public GameObject scopeImage;

    public Text timertext;

    [SerializeField] private GameObject gameoverScreen;
    private GameObject _gameoverScreen;

    public static float currentTimer;
    private static bool timerState;

    [SerializeField] private GameObject playerSpawnPoint;
    [SerializeField] private GameObject cheatCodesMenu;

    public PlayerUI playerUI = new PlayerUI();



    [Header("Aselista. Kauppa ottaa täältä aseen ja antaa sen pelaajalle.")]
    public List<WeaponPrefab> weapons = new List<WeaponPrefab>();

    [Header("Level tier")]
    public List<LevelTier> levelTier = new List<LevelTier>();

    public static float largestBulletSpread; // Toimii Maximi lukunua pelaajan aseiden tarkkuuus vertailussa.

    private static GameObject player;
    private Transform canvas;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        weapons = SortWeaponsToPriceOrder();

        // Check if no spawnpoint
        if(GameObject.FindGameObjectWithTag("playerspawnpoint") == null)
        {
            Transform s = Instantiate(playerSpawnPoint).transform;
            s.transform.position = new Vector3(0, 0, 0);
            s.tag = "playerspawnpoint";
        }
    }

    private void Start()
    {
        Timer(true);

        for(int i=0; i < weapons.Count; i++)
        {
            if (weapons[i].weaponStats.bulletSpread > largestBulletSpread)
                largestBulletSpread = weapons[i].weaponStats.bulletSpread;

            if (!weapons[i].useCustomAdjustments)
                weapons[i] = WeaponPrefab.LoadPrefabInfo(weapons[i]);
        }

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bullet"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Shell"));

        //currentTimer = 1000;

        //TogglePause();

        //GameObject.FindGameObjectWithTag("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        /*
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        */

        // Load defaults
        //Colors.ShopColors.LoadDefaults();
        //weapons = Weapon.InitializeWeapon(weapons);
        RestartGame();
    }

    public static void Timer(bool state)
    {
        timerState = state;
    }

    public void ResetTimer()
    {
        currentTimer = 0;
    }

    private void UpdateTime()
    {
        if (timertext == null || !timerState)
            return;

        currentTimer += Time.deltaTime;

        timertext.text = Utility.GetFormattedTimeFromSeconds(currentTimer);

        //print((int)currentTimer / 5);
        int maxEnemies = (int)currentTimer / 5;
        Enemies.Instance.maxEnemyCoumt = maxEnemies;
    }

    private List<WeaponPrefab> SortWeaponsToPriceOrder()
    {
        List<WeaponPrefab> listThatWillBeReturned = new List<WeaponPrefab>();
        WeaponPrefab w;

        for(int i=0; i < weapons.Count; i++)
        {
            w = this.weapons[i];

            for(int j=0; j < weapons.Count; j++)
            {             
                if(i != j)
                {
                    if(w.price > weapons[j].price)
                    {
                        w = weapons[j];
                    }

                    //print("comapring: " + weapons[i].weaponName + " --> " + weapons[j].weaponName);
                }
            }

            //print("removed: " + w.weaponName);
            listThatWillBeReturned.Add(w);
            weapons.Remove(w);
        }

        listThatWillBeReturned.Add(weapons[0]);
        return listThatWillBeReturned;
    }

    public bool ShopOpen()
    {
        return _shopUI != null;
    }

    public WeaponStats GetWeaponStats(string weaponName)
    {
        for(int i=0; i < weapons.Count; i++)
        {
            if(weapons[i].weaponName == weaponName)
            {
                return weapons[i].weaponStats;
            }
        }

        return null;
    }

    public void OpenShop()
    {
        if(_highscoreUI != null)
        {
            Destroy(_highscoreUI);
            return;
        }

        if(_pausemenu != null)
        {
            Destroy(_pausemenu);
            return;
        }

        if (_shopUI != null)
        {
            if (_shopUI != null)
                Destroy(_shopUI);

            FPSControllerHandler(false);
        }
        else
        {
            _shopUI = Instantiate(shopUI);
            _shopUI.transform.SetParent(canvas, false);

            if(_pausemenu != null)
            {
                Destroy(_pausemenu);
            }

            FPSControllerHandler(true);
        }
    }

    public void HighscoreMenu()
    {
        if(_highscoreUI == null)
        {
            // avaa
            _highscoreUI = Instantiate(highscoreUI);
            _highscoreUI.transform.SetParent(canvas, false);
            _highscoreUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(1, 1);
        }
        else 
        {
            Destroy(_highscoreUI);
        }
    }

    public void Gameover(bool state)
    {
        if(_gameoverScreen == null && state)
        {
            // Close shop
            if(_shopUI != null)
            {
                Destroy(_shopUI);
            }

            if(_highscoreUI != null)
            {
                Destroy(_highscoreUI);
            }

            if(_pausemenu != null)
            {
                Destroy(_pausemenu);
            }

            _gameoverScreen = Instantiate(gameoverScreen);
            _gameoverScreen.transform.SetParent(canvas, true);
            _gameoverScreen.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
           // if (!_gameoverScreen.activeSelf)
             _gameoverScreen.SetActive(true);

            FPSControllerHandler(true);
            Timer(false);
        }
        else if(!state)
        {
            Destroy(_gameoverScreen);
            RestartGame();
        }
    }



    public static void FPSControllerHandler(bool state)
    {
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = !state;

        if (state)
        {
            GameManager.pause = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            GameManager.pause = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void OpenPausemenu()
    {
        if(_pausemenu == null)
        {
            // Avaaa
            _pausemenu = Instantiate(pausemenu);
            _pausemenu.transform.SetParent(canvas, false);
            FPSControllerHandler(true);
        }
        else if(_pausemenu != null)
        {
            // Sulje
            Destroy(_pausemenu);
            FPSControllerHandler(false);
        }
    }

    public void Map(bool state)
    {
        if (state)
        {
            if(_map == null)
            {
                if (ShopOpen())
                    return;

                _map = Instantiate(map);
                _map.transform.SetParent(canvas);
                _map.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }
        }
        else
        {
            if (_map != null)
                Destroy(_map);
        }
    }

    public static void RestartGame()
    {
        Player.money = 0;
        Player.Money(0); // update ui
        Player.currentHealth = Player.maxHealth;
        Player.Damage(0);
        currentTimer = 0;
        Player.score = 0;
        Player.alive = true;
        Player.ammoHit = 0;
        Player.ammoUsed = 0;
        Player.enemiesKilled = 0;
        pause = false;

        Player.Instance.weapons.Clear();

        player.transform.position = GameObject.FindGameObjectWithTag("playerspawnpoint").transform.position;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        for(int i=0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }

        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        for (int i = 0; i < weapons.Length; i++)
        {
            Destroy(weapons[i]);
        }

        Player.Instance.LoadWeapons();

        FPSControllerHandler(false);

        Timer(true);
    }

    private void Update()
    {
        UpdateTime();

        if (ShopOpen())
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!cheatCodesMenu.activeSelf)
                    cheatCodesMenu.SetActive(true);
                else
                    cheatCodesMenu.SetActive(false);
            }
        }
    }
    
}
