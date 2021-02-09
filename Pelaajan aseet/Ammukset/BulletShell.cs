using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShell : MonoBehaviour {

    private void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Shell"), LayerMask.NameToLayer("Bullet"), true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //this.gameObject.layer = LayerMask.NameToLayer("Default");
    }

}
