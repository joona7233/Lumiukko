using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenMenu : MonoBehaviour {

    [System.Serializable] private class Menu
    {
        public enum Mode { open, close };
        public Mode mode;
        public GameObject targetMenu;
    }

    [System.Serializable] private class StoreInfo
    {
        public string name;
        public GameObject gameObject;
    }

    [SerializeField] private List<Menu> menu = new List<Menu>();
    [SerializeField] private List<StoreInfo> storeInfo = new List<StoreInfo>();

    private Color32 hover, inactive, targetColor;

    private bool colorAnimation;

    private void Start()
    {
        //hover = new Color32(255, 255, 255, 255);
        //inactive = new Color32(255, 255, 255, 125);

        if(this.GetComponent<Text>() != null)
        {
            Color32 c = this.GetComponent<Text>().color;
            hover = new Color32(c.r, c.g, c.b, 255);
            inactive = new Color32(c.r, c.g, c.b, 125);
            this.GetComponent<Text>().color = inactive;
        }

        //Cursor.lockState = CursorLockMode.Confined;
    }

    private StoreInfo GetStoreInfo(string name)
    {
        for(int i=0; i < storeInfo.Count; i++)
        {
            if (storeInfo[i].name == name)
                return storeInfo[i];
        }

        return null;
    }

    public void ToggleLocalMenu()
    {
        for(int i=0;  i < menu.Count; i++)
        {
            if(menu[i].mode == Menu.Mode.open)
            {
                menu[i].targetMenu.SetActive(true);
            }
            else if(menu[i].mode == Menu.Mode.close)
            {
                if(this.GetComponent<Text>() != null)
                {
                    this.GetComponent<Text>().color = inactive;
                }

                colorAnimation = false;

                menu[i].targetMenu.SetActive(false);
            }
        }
    }

    public void MouseEnter()
    {
        //this.GetComponent<Text>().color = hover;
        TargetColor(hover);
    }

    public void MouseExit()
    {
        //this.GetComponent<Text>().color = inactive;
        TargetColor(inactive);
    }

    void TargetColor(Color32 c)
    {
        targetColor = c;
        colorAnimation = true;
    }

    bool CompareInts(int a, int b, int between)
    {
        if (a >= b - between && a <= b + between)
            return true;

        return false;
    }

    bool CompareColors(Color32 a, Color32 b, int between)
    {
        if (CompareInts(a.r, b.r, between))
            if (CompareInts(a.g, b.g, between))
                if (CompareInts(a.b, b.b, between))
                    if (CompareInts(a.a, b.a, between))
                        return true;

        return false;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main menu");
    }

    Coroutine coroutine_saveHighScore;
    public void SaveHighscore()
    {
        //ToggleLocalMenu();
        if(coroutine_saveHighScore == null)
            coroutine_saveHighScore = StartCoroutine(_SaveHighScore());
    }

    public void ContinueWithoutSavingHighscore()
    {
        ToggleLocalMenu();
    }

    public void RestartGame()
    {
        GameManager.RestartGame();
        Destroy(this.GetComponentInParent<Gameover>().gameObject);
    }

    public void ReturnToMainMenu()
    {
        //GameManager.RestartGame();
        SceneManager.LoadScene("Main menu");
    }

    IEnumerator _SaveHighScore()
    {
        string username = GetStoreInfo("name").gameObject.GetComponent<InputField>().text;

        float currentTime = 0f;
        float maxTime = 5f;

        if (username == "")
        {
            GetStoreInfo("name").gameObject.GetComponent<InputField>().text = Utility.GetRandomUsername();
            yield break;
        }

        ToggleLocalMenu();

        //StartCoroutine(Highscore.SubmitHighscore(username, Player.score, Player.accuracy, Player.ammoUsed, Player.money, GameManager.currentTimer));
        StartCoroutine(Highscore.SubmitHighscore(username, Player.score, ((float)Player.ammoUsed /(float)Player.ammoHit).ToString("F2"), Player.ammoUsed, Player.enemiesKilled, Player.money, GameManager.currentTimer));

        while (Highscore.submit_respond != "true")
        {
            currentTime += Time.deltaTime;
            print("ct: " + currentTime + " / " + maxTime);

            if (currentTime >= maxTime)
            {
                Finish();
                yield break;
            }

            yield return null;
        }

        Finish();

        yield return null;
    }

    void Finish()
    {
        if (Highscore.submit_respond == "true")
        {
            // onnistui
            menu[0].targetMenu.SetActive(false);
            menu[0].targetMenu.GetComponentInChildren<Text>().text = "Onnistui!";
            menu[0].targetMenu.GetComponentsInChildren<Image>()[2].enabled = false;

            GameManager.Instance.HighscoreMenu();

            GetStoreInfo("screen2").gameObject.SetActive(false);

            GetStoreInfo("restart").gameObject.SetActive(true);

            print("!!!!");
        }
        else
        {
            // epäonnistui
            menu[0].targetMenu.SetActive(false);
            menu[0].targetMenu.GetComponentInChildren<Text>().text = "Onnistui!";
            menu[0].targetMenu.GetComponentsInChildren<Image>()[2].enabled = false;

            GetStoreInfo("screen2").gameObject.SetActive(false);

            GetStoreInfo("restart").gameObject.SetActive(true);

            print("?");
        }

        coroutine_saveHighScore = null;
    }

    public void RandomizeName()
    {
        GetStoreInfo("name").gameObject.GetComponent<InputField>().text = Utility.GetRandomUsername();
    }

    public void DestroyHighscoreParent()
    {
        if (this.GetComponentInParent<Highscore>() != null)
            Destroy(this.GetComponentInParent<Highscore>().gameObject);
    }

    private void Update()
    {
        if (colorAnimation)
        {
            this.GetComponent<Text>().color = Color32.Lerp(this.GetComponent<Text>().color, targetColor, 2.5f * Time.deltaTime);

            if (CompareColors(this.GetComponent<Text>().color, targetColor, 30))
                colorAnimation = false;
        }
    }

}
