using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuOpenScoreboard : MonoBehaviour {

    [SerializeField] GameObject scoreboard;
    
    public void OpenScoreBoard()
    {
        RectTransform r = Instantiate(scoreboard.GetComponent<RectTransform>());
        //r.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
        r.SetParent(GameObject.FindGameObjectWithTag("hscore").transform,  true);
        r.sizeDelta = new Vector2(1, 1);
        r.SetAsLastSibling();
        r.anchoredPosition = new Vector2(0, 0);

    }

}
