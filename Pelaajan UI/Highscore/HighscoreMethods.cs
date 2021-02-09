using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreMethods : MonoBehaviour {

    [SerializeField] private RectTransform detailedInfO;

    private Color32 hover;
    private Color32 normal;

    private Color32 reddishHover;
    private Color32 reddishNormaL;

    private void Start()
    {
        hover = new Color32(167, 255, 191, 255);
        normal = new Color32(98, 164, 116, 118);

        reddishNormaL = new Color32(255, 93, 99, 118);
        reddishHover = new Color32(255, 71, 78, 255);
    }

    public void OnHover()
    {
        detailedInfO.gameObject.SetActive(true);
    }

    public void OnExit()
    {
        detailedInfO.gameObject.SetActive(false);
    }

    public void OnHover1()
    {
        this.GetComponent<Text>().color = hover;
    }

    public void OnExit1()
    {
        this.GetComponent<Text>().color = normal;
    }

    public void OnHover2()
    {
        this.GetComponent<Text>().color = reddishHover;
    }

    public void OnExit2()
    {
        this.GetComponent<Text>().color = reddishNormaL;
    }

    public void ContinueWithoutSaving()
    {

    }

    public void ContinueWithSave()
    {
        
    }
}
