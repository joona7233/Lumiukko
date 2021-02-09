using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatCodes : MonoBehaviour {

    InputField i;

	void OnEnable () {
        i = this.GetComponent<InputField>();
        this.GetComponent<RectTransform>().SetAsLastSibling();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (i.text == "")
                return;

            if (i.text == "money")
            {
                Player.Money(214700000);
            }

            if (i.text == "immunity")
            {
                Player.immunity = !Player.immunity;
            }

            i.text = "";
        }

	}
}
