using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldCheckLength : MonoBehaviour {

    [SerializeField] private int maxTextLength;

    public void CheckLength()
    {
        if(this.GetComponent<InputField>().text.Length >= maxTextLength)
        {
            
        }
    }

}
