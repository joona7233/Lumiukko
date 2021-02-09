using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scope {

    public bool enabled;

    public float zoomStep;

    public float nornal_fieldOfView;
    public float current_fieldOfView;
    public float maxZoom_fieldOfView;

    // Animation fix 
    public float maxAnimationTime;
    [HideInInspector] public float currentAnimationTime;

    //public Camera camera;
    public Animator animator;

    public GameObject scopeImage;
    public GameObject _scopeImage;

    public void ResetFieldOfView(float value)
    {
        current_fieldOfView = value;
        nornal_fieldOfView = value;
    }

    public void MoveScope(float bulletSpread, float _multiplierInc)
    { 
        if(_scopeImage != null)
        {
            float multiplier = 2f + _multiplierInc; //10                                  
            _scopeImage.GetComponent<RectTransform>().anchoredPosition3D = new Vector2(Random.Range(-bulletSpread * multiplier, bulletSpread * multiplier), Random.Range(-bulletSpread * multiplier, bulletSpread * multiplier));
        }
    }

    public void ResetScope(float speed)
    {
        if (_scopeImage == null)
            return;

        if(_scopeImage.GetComponent<RectTransform>().anchoredPosition3D != Vector3.zero)
            _scopeImage.GetComponent<RectTransform>().anchoredPosition3D = Vector2.Lerp(_scopeImage.GetComponent<RectTransform>().anchoredPosition3D, new Vector2(0, 0), speed * Time.deltaTime);
    }

}
