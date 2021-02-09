using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {

    [SerializeField] private float Time;

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyAfterSeconds(Time));
	}

    private IEnumerator DestroyAfterSeconds(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        Destroy(this.gameObject);
    }
}
