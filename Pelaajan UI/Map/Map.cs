using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    [SerializeField] private new Text name;
    [SerializeField] private Text score;
    [SerializeField] private Text accuracy;
    [SerializeField] private Text ammo_used;

    private void OnEnable()
    {
        float acc = (float)Player.ammoHit / (float)Player.ammoUsed;

        name.text = Player.name;
        score.text = Player.score.ToString();
        accuracy.text = acc.ToString("F2") + "%";
        ammo_used.text = Player.ammoUsed.ToString();
    }

}
