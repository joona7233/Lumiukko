using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateShopWeapon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //this.transform.Rotate(new Vector3(0, 50, 0), 90.5f * Time.deltaTime);
        this.transform.Rotate(new Vector3(0, 0, 5f), 25 * Time.deltaTime, Space.Self);
	}
}
