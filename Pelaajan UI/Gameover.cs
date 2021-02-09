using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameover : MonoBehaviour {

    [System.Serializable] private class Screen1
    {
        public Text score;
        public Text time;
        public Text enemiesKilled;
    }

    [SerializeField] GameObject loadingIndicator;
    [SerializeField] Text loadingText;

    [SerializeField] Button continueButton;
    [Header("")]
    public GameObject loadingIndicatorMenu;
    [Header("")]
    public GameObject restartmenu;

    [Header("")]

    [SerializeField] private Screen1 screen1 = new Screen1();

    void Start () {
        //StartCoroutine(SubmitScore());

        screen1.score.text = Player.score+"";
        screen1.time.text = Utility.GetFormattedTimeFromSeconds(GameManager.currentTimer);
        screen1.enemiesKilled.text = Player.enemiesKilled + "";
	}

    /*
    IEnumerator SubmitScore()
    {
        float currentTimer = 0;
        float maxTimer = 25;
        string dots = "";

        int dotCount = 5;
        float dotTimer = 0;
        float max_dotTimer = .25f;

        // Enablee lataus indicaattori ja teksti.
        if (!loadingIndicator.activeSelf)
            loadingIndicator.SetActive(true);

        if (!loadingText.gameObject.activeSelf)
            loadingText.gameObject.SetActive(true);

        while (true)
        {
            currentTimer += Time.deltaTime;
            dotTimer += Time.deltaTime;

            loadingIndicator.transform.Rotate(new Vector3(0, 0, -25f), 25f * 50f * Time.deltaTime);

            if(dotTimer >= max_dotTimer)
            {
                dots += ".";
                dotTimer = 0;
            }

            if(dots.Length >= dotCount+1)
            {
                dots = "";
            }

            loadingText.text = "Yhdistetaan" + dots;

            if(currentTimer >= maxTimer)
            {
                
                yield break;
            }

            yield return null;
        }
    }
    */

}
