using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class Utility {

    public static string GetRandomUsername()
    {
        StreamReader s = new StreamReader("Assets/usernames.txt");
        string all = s.ReadToEnd();
        string[] lines = all.Split(new string[] { "\n"}, System.StringSplitOptions.None);
        s.Close();
        return lines[Random.Range(0, lines.Length)];
    }

    public static Transform FindTransformFromChildren(string transformName, Transform targetParent)
    {
        Transform[] t = targetParent.GetComponentsInChildren<Transform>();
        foreach(Transform _t in t)
        {
            if(_t.name == transformName)
            {
                return _t;
            }
        }

        return null;
    }

    public static RectTransform FindRectTransformFromChildren(string transformName, RectTransform targetParent)
    {
        RectTransform[] t = targetParent.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform _t in t)
        {
            if (_t.name == transformName)
            {
                return _t;
            }
        }

        return null;
    }

    public static Text FindTextFromChildren(string transformName, RectTransform targetParent)
    {
        Text[] t = targetParent.GetComponentsInChildren<Text>();
        foreach (Text _t in t)
        {
            if (_t.name == transformName)
            {
                return _t;
            }
        }

        return null;
    }

    public static string[] StringToStringArray(string targetText, string separator)
    {
        List<string> list_s = new List<string>();

        string[] splittedText = targetText.Split(new string[] { separator }, System.StringSplitOptions.None);

        for (int i=0; i < splittedText.Length-1; i++)
        {
            list_s.Add(splittedText[i].Replace(separator, "").Trim());            
        }

        return list_s.ToArray();
    }

    public static Vector3 GetRandomPointFromObject(GameObject g)
    {
        return new Vector3(Random.Range(-g.transform.localScale.x, g.transform.localScale.x), g.transform.position.y, Random.Range(-g.transform.localScale.z, g.transform.localScale.z));
    }

    public static string GetFormattedTimeFromSeconds(float currentTimer)
    {
        int minutes = 0;
        int seconds = 0;

        minutes = ((int)currentTimer / 60);
        seconds = (int)currentTimer % 60;

        string text = "";

        if (minutes >= 10 && seconds >= 10)
        {
            text = minutes + ":" + seconds;
        }
        else if (minutes < 10 && seconds >= 10)
        {
            text = "0" + minutes + ":" + seconds;
        }
        else if (minutes >= 10 && seconds < 10)
        {
            text = minutes + ":" + "0" + seconds;
        }
        else if (minutes < 10 && seconds < 10)
        {
            text = "0" + minutes + ":" + "0" + seconds;
        }

        return text;
    }

}
