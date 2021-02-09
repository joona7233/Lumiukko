using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {

    [SerializeField] private RectTransform listitem;
    [SerializeField] private RectTransform listItemInfo;
    [SerializeField] private RectTransform listItemParent;

    [SerializeField] private Scrollbar scrollBar;

    public static string submit_respond;

    private void Start()
    {
        //StartCoroutine(SubmitHighscore("Mmeoow", 7123, 55.256f, 2555, 12565, 32.52f));
        StartCoroutine(LoadHighscores());

        //StartCoroutine(SubmitHighscore());
    }

    private void CreateListItem(string[] s)
    {
        for(int i=0; i < s.Length; i++)
        {
            string name = s[i].Split(':')[0];
            string time = s[i].Split(':')[1];
            string score = s[i].Split(':')[2];
            string accuracy = s[i].Split(':')[3];
            string ammo_used = s[i].Split(':')[4];
            string money_left = s[i].Split(':')[5];
            string enemiesKilled = s[i].Split(':')[6];

            /*
            print(score);
            print(accuracy);
            print(ammo_used);
            print(money_left);
            */

            RectTransform _listitem = Instantiate(listitem);

            // Anna positioni & muuta kokoa
            _listitem.gameObject.SetActive(true);
            _listitem.parent = listItemParent;
            _listitem.sizeDelta = new Vector2(listitem.sizeDelta.x, listitem.sizeDelta.y);
            _listitem.anchoredPosition = new Vector2(1, _listitem.sizeDelta.y * -i);

            listItemParent.sizeDelta += new Vector2(0, (_listitem.sizeDelta.y));

            /*
            RectTransform _listItemInfo = Instantiate(listItemInfo);
            _listItemInfo.gameObject.SetActive(true);
            _listItemInfo.sizeDelta = new Vector2(listItemInfo.sizeDelta.x, listItemInfo.sizeDelta.y);
            _listItemInfo.anchoredPosition = new Vector2(1, Input.mousePosition.y);
            */

           // _listitem.position = new Vector2(0, _listitem.sizeDelta.y * -i);

            // -r.sizeDelta.y * shopItems.Count) - r.sizeDelta.y / 2

            // Muuta arvot oikeiksi
            Utility.FindTextFromChildren("name", _listitem).text = name;
            Utility.FindTextFromChildren("time", _listitem).text = Utility.GetFormattedTimeFromSeconds(float.Parse(time));
            Utility.FindTextFromChildren("rank", _listitem).text = (i + 1).ToString();

            Utility.FindTextFromChildren("score", _listitem).text = score;
            Utility.FindTextFromChildren("money", _listitem).text = money_left;
            Utility.FindTextFromChildren("accuracy", _listitem).text = accuracy + "%";
            Utility.FindTextFromChildren("ammo", _listitem).text = enemiesKilled;

            Utility.FindRectTransformFromChildren("DetailedInfo", _listitem).gameObject.SetActive(false);
        }

        scrollBar.value = 1;
    }

    private IEnumerator LoadHighscores()
    {
        WWW www = new WWW("http://joonaohberg.com/HAMK/lumiukko-php/php/query/load.php");
        yield return www;

        CreateListItem(Utility.StringToStringArray(www.text, "<br>"));

        /*
        // test
        string[] h = Utility.StringToStringArray(www.text, "<br>");
        for(int i=0; i < h.Length; i++)
        {
            print(h[i]);
        }
        */
    }

    public static IEnumerator SubmitHighscore(string name, int score, string accuracy, int ammoUsed, int enemiesKilled, int money, float time)
    {
        submit_respond = "";

        WWWForm form = new WWWForm();

        form.AddField("username", name);
        form.AddField("score", score);
        form.AddField("accuracy", accuracy.ToString());
        form.AddField("ammo_used", ammoUsed);
        form.AddField("money_left", money);
        form.AddField("time", time.ToString("F2"));
        form.AddField("enemies_killed", Player.enemiesKilled);

        WWW www = new WWW("http://joonaohberg.com/HAMK/lumiukko-php/php/query/submit.php", form);

        yield return www;

        if (www.text.Trim() != "true")
            print("Error: " + www.text);

        submit_respond = www.text;
    }



}
