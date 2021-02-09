using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

	void Update () {
        this.transform.Rotate(new Vector3(0, 0, -25f), 25f * 50f * Time.deltaTime);
    }
}
