using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int damage;
    public Vector3 bulletSpeed;
    [SerializeField] private ParticleSystem spark;
    [SerializeField] private GameObject bulletHole;
    private GameObject player, gamemanager;

    Rigidbody rb;
    bool hit;

    public static Color32 hitColor;

    //void Start () {
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gamemanager = GameObject.FindGameObjectWithTag("gamemanager");

        rb = this.GetComponent<Rigidbody>();
        rb.AddForce(bulletSpeed);

        this.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(RunAfterCertainSeconds(0.025f));

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"), true);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("InvisibleWall"), true);
    }
	
    IEnumerator RunAfterCertainSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!this.GetComponent<MeshRenderer>().enabled)
            this.GetComponent<MeshRenderer>().enabled = true;

        rb.AddExplosionForce(Random.Range(125, 255), collision.transform.position, 15);

        // Instnatiate particle effect
        ParticleSystem _spark = Instantiate(spark);
        var main = _spark.main;
        MeshRenderer m = collision.gameObject.GetComponent<MeshRenderer>();
        if (m != null)
            main.startColor = m.material.color;

        _spark.transform.parent = null;
        _spark.transform.position = this.transform.position;
        _spark.transform.rotation = Quaternion.LookRotation(rb.velocity);
         Destroy(_spark.gameObject, 2f);

        // Katostaan että osuttiinko viholliseen! Eli jokaisen vihollisen täytyy sisältää "Enemy" componentti
        Component enemyScript = collision.gameObject.GetComponent<Enemy>();

        if(enemyScript == null)
        {
            enemyScript = collision.gameObject.GetComponentInParent<Enemy>();
        }

        if(enemyScript != null)
        {
            
            if (collision.gameObject.name.ToLower().Trim() == "head")
            {
                // Pääosuma
                int _damage = damage * 2;
                
                enemyScript.GetComponent<Enemy>().health -= _damage;
                hitColor = new Color32(255, 28, 0, 255);
                gamemanager.GetComponent<GameManager>().hitUI.SetActive(true);
                HitUI.Initialize();
                
            }
            else
            {
                // Normi osuma
                int _damage = damage;

                enemyScript.GetComponent<Enemy>().health -= _damage;
                //print(collision.gameObject.name);
                hitColor = new Color32(255, 137, 0, 255);
                GameManager.Instance.hitUI.SetActive(true);
                HitUI.Initialize();
               
            }

            Player.ammoHit++;
        }
        else
        {
            // Spawn bullet hole
            if (collision.gameObject.tag != "Player" || collision.gameObject.tag != "Bullet" || collision.gameObject.tag != "Shell")
            {
                GameObject _bulletHole = Instantiate(bulletHole);
                Vector3 rotation = new Vector3(collision.transform.rotation.x, collision.transform.rotation.y, collision.transform.rotation.z + 90f);
                // _bulletHole.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90)); 
                _bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
                Vector3 dir = collision.contacts[0].point - player.transform.position;
                _bulletHole.transform.position = collision.contacts[0].point + (-dir * .02f);
                _bulletHole.transform.parent = collision.transform;
                Destroy(_bulletHole, 3f);
            }
        }

        Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
    }

}
