using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        Enemies.Instance.SpawnRandomEnemy(this.transform.localPosition);
        Destroy(this.gameObject);
    }

}
