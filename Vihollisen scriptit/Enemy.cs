using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Rewards")]
    public int rewardMoney;
    public int rewardScore;

    [Header("Stats")]
    public int health; // Bullet osuma vähentää tästä.

    [SerializeField] private ParticleSystem snowDeathEffect;

    private void Start()
    {
        /*
        rbs = this.GetComponentsInChildren<Rigidbody>();
        for(int i = 1; i < rbs.Length; i++)
        {
            rbs[i].isKinematic = true;
            rbs[i]
        }
        */
    }

    void Update()
    {
        if (health <= 0)
        {
            //Invoke("Die", 3f);

            //OnDeath();
            Die();
        }
    }

    void OnDeath()
    {
        this.GetComponent<Animator>().Play("death");
        NavMeshAgent nav = this.GetComponent<NavMeshAgent>();
        nav.speed = 0;

        /*
        Rigidbody rb = this.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.drag = 250f;
            rb.velocity = Vector3.zero;
        }
        */
    }

   void Die()
    {
        Player.Instance.Reward(this.GetComponent<Enemy>().rewardMoney, this.GetComponent<Enemy>().rewardScore);

        if(snowDeathEffect != null)
        {
            ParticleSystem p = Instantiate(snowDeathEffect);
            p.transform.position = this.transform.position + new Vector3(0, 5f, 0);
            Destroy(p, 3f);
        }

        Destroy(this.gameObject);            
    }

    void Boom()
    {
        /*
        Rigidbody[] rbs = this.GetComponentsInChildren<Rigidbody>();

        for(int i=1; i < rbs.Length; i++)
        {
            rbs[i].isKinematic = false;
            rbs[i].AddExplosionForce(5f, this.transform.position, 15f);
            rbs[i].transform.parent = null;
            Destroy(rbs[i].gameObject, 4.5f);
        }*/
        /*
        Transform[] ts = this.GetComponentsInChildren<Transform>();
        for(int i=1; i < ts.Length; i++)
        {
            if (ts[i].gameObject.name != "body")
                return;

            ts[i].gameObject.AddComponent<Rigidbody>();
            ts[i].GetComponent<Rigidbody>().isKinematic = false;
            //ts[i].GetComponent<Rigidbody>().AddExplosionForce(5f, this.transform.position, 2);

            ts[i].gameObject.AddComponent<MeshCollider>();
            ts[i].GetComponent<MeshCollider>().convex = false;

            ts[i].transform.parent = null;

            Destroy(ts[i].gameObject, 2.5f);
        }
        */
    }

    #region Joona
    /*
     * 
    public int rewardMoney;
    public int rewardScore;
    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
        }
    }
    */

    /*
    [System.Serializable]
    public class Enemy : MonoBehaviour
    {




    public int speed;

    public bool useGun;
    public float stoppingDistance;

    private GameObject gamemanager;

    [SerializeField] private GameObject rewardMoney_textSplash;

    public void InitializeEnemy()
    {
        gamemanager = GameObject.FindGameObjectWithTag("gamemanager");
        rewardMoney_textSplash = gamemanager.GetComponent<GameManager>().scoreSplash;
    }

    public void InstantiateMoneyText()
    {
        // Instantiate + text for Money
        GameObject scoreText = Instantiate(rewardMoney_textSplash);
        scoreText.GetComponent<TextMesh>().text = "+" + rewardMoney;
        scoreText.gameObject.name = "######################################";
        scoreText.transform.position = this.transform.position;

        // Add score
        Player.Money += rewardMoney;
        Player.Score += rewardScore;
    }

    }
    */
    #endregion
}