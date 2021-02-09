using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitUI : MonoBehaviour {

    private static byte alpha;
    private static Color32 currentColor;

    public static void Initialize()
    {
        alpha = 255;
        currentColor = Bullet.hitColor;
    }

    // Update is called once per frame
    void Update () {
        alpha -= 5;

        if (alpha <= 0)
        {
            this.gameObject.SetActive(false);
        }

        currentColor = new Color32(currentColor.r, currentColor.g, currentColor.b, alpha);
        this.GetComponent<Image>().color = currentColor; 	

	}
}
