using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    private GameObject gamemanager;

    [SerializeField] private GameObject shopitem;
    [SerializeField] private GameObject shopitem_alreadyBought;

    [SerializeField] private GameObject scrollRect_weapons;

    [SerializeField] private Text money;

    private List<ShopItem> shopItems = new List<ShopItem>();

    private class ShopItem
    {
        public GameObject shopItem;
        public WeaponPrefab w;

        public ShopItem(GameObject shopItem, WeaponPrefab w)
        {
            this.shopItem = shopItem;
            this.w = w;
        }
    }

    public static Shop Instance;

    private void Awake()
    {
        Instance = this;
        gamemanager = GameObject.FindGameObjectWithTag("gamemanager");
    }

    private void Start()
    {
        LoadWeapons();
        UpdateShopMoney();
    }

    public void UpdateShopMoney()
    {
        money.text = Player.money.ToString();
    }

    void SpawnWeaponMenu(int i)
    {
        GameObject _shopItem = Instantiate(shopitem);

        if (!_shopItem.activeSelf)
            _shopItem.SetActive(true);

        RectTransform r = _shopItem.GetComponent<RectTransform>();
        _shopItem.transform.SetParent(scrollRect_weapons.transform);

        // Aseta paikka ja koko
        _shopItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-r.sizeDelta.y * shopItems.Count) - r.sizeDelta.y / 2);
        r.sizeDelta = new Vector2(0, r.sizeDelta.y);

        // Aseta aseen nimi ja teksti
        WeaponPrefab w = gamemanager.GetComponent<GameManager>().weapons[i];
        FindTextFromParent("weapon-name", _shopItem.transform).text = w.weaponName;
        FindTextFromParent("weapon-info", _shopItem.transform).text =
            "Vahinko pisteet: " + w.weaponStats.damage * w.weaponStats.bulletsPerShot + "\n" +
            "Tarkkuus: " + (100 - ((w.weaponStats.bulletSpread / GameManager.largestBulletSpread) * 100)) + "%" + "\n" +
            "Ampumisnopeus: " + (w.weaponStats.rateOfFire) + "\n" +
            "Latausaika: " + (w.weaponStats.reloadTime);

        // Aseta hinta
        _shopItem.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = w.price.ToString();

        // Aseta aseen pikku kuva
        _shopItem.GetComponentInChildren<RawImage>().texture = w.shopImage;

        //InstantiatedShopItems[i] = _shopItem;

        ShopItem s = new ShopItem(_shopItem, w);
        shopItems.Add(s);
    }

    void SpawnWeaponAmmoMenu(int i)
    {
        // Instantiatea (buy ammo menu...)
        GameObject _shopItem = Instantiate(shopitem_alreadyBought);

        if (!_shopItem.activeSelf)
            _shopItem.SetActive(true);

        RectTransform r = _shopItem.GetComponent<RectTransform>();
        _shopItem.transform.SetParent(scrollRect_weapons.transform);

        // Aseta paikka ja koko
        _shopItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-r.sizeDelta.y * shopItems.Count) - r.sizeDelta.y / 2);
        r.sizeDelta = new Vector2(0, r.sizeDelta.y);

        // Aseta aseen nimi ja teksti
        WeaponPrefab w = gamemanager.GetComponent<GameManager>().weapons[i];

        FindTextFromParent("weapon-name", _shopItem.transform).text = w.weaponName;

        // Aseta aseen pikku kuva
        _shopItem.GetComponentInChildren<RawImage>().texture = w.shopImage;

        ShopItem s = new ShopItem(_shopItem, w);
        shopItems.Add(s);
    }

    public void LoadWeapons()
    {
        if(shopItems.Count > 0)
        {
            foreach (ShopItem s in shopItems)
                Destroy(s.shopItem);

            shopItems.Clear();
        }

        GameObject[] InstantiatedShopItems = new GameObject[gamemanager.GetComponent<GameManager>().weapons.Count];
        //ShopItem[] shopItem = new ShopItem[gamemanager.GetComponent<GameManager>().weapons.Count];

        for(int i=0; i < gamemanager.GetComponent<GameManager>().weapons.Count; i++)
        {
            if(!gamemanager.GetComponent<GameManager>().weapons[i].owned)
            {
                SpawnWeaponMenu(i);
            }
            else
            {
                SpawnWeaponAmmoMenu(i);
            }
        }

        // Järjestä kauppa tavarat hinta (pienimmästä --> suurimpaan)
        //List<ShopItem> shopItemsLeftToInstantiate = shopItems;
        /*
        ShopItem item = new ShopItem(new GameObject(), new WeaponPrefab());

        for(int i=0; i < shopItems.Count; i++)
        {
            for(int j=0; j < shopItems.Count; j++)
            {
                if(shopItems[i].w.price > item.w.price)
                {
                    if(i != j)
                    {
                        // Etsi halvin ase
                        if(item.w.price > shopItems[i].w.price)
                        {
                            item = shopItems[i];
                        }
                    }
                }
            }

            // Aseta paikka "shopItem"ille.
            RectTransform r = item.shopItem.GetComponent<RectTransform>();
            item.shopItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-r.sizeDelta.y * i) - r.sizeDelta.y / 2);
        }
        */
        /*
        for(int i=0; i < shopItem.Length; i++)
        {
            for(int j=0; j < shopItem.Length; j++)
            {
                if(i != j)
                {
                    if(shopItem[i].w.price > shopItem[j].w.price)
                    {
                        GameObject storeShopItemPosition = shopItem[i].shopItem;

                        shopItem[i].shopItem.GetComponent<RectTransform>().anchoredPosition = shopItem[j].shopItem.GetComponent<RectTransform>().anchoredPosition;
                        shopItem[j].shopItem.GetComponent<RectTransform>().anchoredPosition = storeShopItemPosition.GetComponent<RectTransform>().anchoredPosition;

                        print("Setting new posiiton for: " + shopItem[i].w.weaponName);
                    }
                }
            }
        }
        */
        


    }

    public static Text FindTextFromParent(string name, Transform shopItem)
    {
        RectTransform[] items = shopItem.GetComponentsInChildren<RectTransform>();
        for (int i=0; i < items.Length; i++)
        {
            if(items[i].gameObject.name == name)
            {
                return items[i].GetComponent<Text>();
            } 
        }

        return null;
    }

    public static Transform FindTransformFromParent(string name, Transform shopItem)
    {
        RectTransform[] items = shopItem.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].gameObject.name == name)
            {
                return items[i];
            }
        }

        return null;
    }

}
