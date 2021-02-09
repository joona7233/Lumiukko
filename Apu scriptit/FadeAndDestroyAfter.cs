using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAndDestroyAfter : MonoBehaviour {

    [SerializeField] private int startFadingAfter;
    [SerializeField] private float fadeStep;
    [SerializeField] private float destroyAfter;
    private float currentTime;

    [SerializeField] private Color32 targetColor;

    bool allowColorTransistion;

    private void Start()
    {
        Invoke("_enable", startFadingAfter);
    }

    void _enable()
    {
        allowColorTransistion = true;
    }

    void Update () {

        if (allowColorTransistion)
        {
            if (this.GetComponent<Text>() != null)
                this.GetComponent<Text>().color = Color32.Lerp(this.GetComponent<Text>().color, targetColor, fadeStep * Time.deltaTime);

            currentTime += Time.deltaTime;
            if(currentTime >= destroyAfter)
            {
                Destroy(this.gameObject);
            }
        }

	}
}
