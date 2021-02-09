using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFunctions : MonoBehaviour {

    public void OpenPauseMenu()
    {        
        if(this.gameObject.name == "Button (1)")
        {
            GameManager.Instance.HighscoreMenu();
        }
        else
        {
            GameManager.Instance.OpenPausemenu();
            //GameManager.Instance.OpenShop();
        }

    }
}
