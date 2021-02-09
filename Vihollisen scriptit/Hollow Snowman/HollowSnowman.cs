using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HollowSnowman : EnemyBehaviour {

    [SerializeField] private bool onGround;
    [SerializeField] private float hitDistance;
    [SerializeField] private Material glowingEyesAndMouth;

    private bool allowFly;

    private float attackRange;

    private enum AttackState { findPlayer, jumpAttack, fly, HitPlayer };
    private AttackState attackState;

    private Rigidbody rb;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = this.GetComponent<NavMeshAgent>();
    }

    void Start () {
        onGround = true;
        attackRange = 25; // millon lumiukko  hyppää pelaajaan kiinni, 35
        hitDistance = 4.5f;
        nav.speed = 25.5f;
	}
	
    void Attack()
    {
        if (attackState == AttackState.findPlayer)
        {
            rb.mass = 1;
            nav.enabled = true;
            nav.SetDestination(player.transform.position);
            onGround = true;
        }

        if (attackState == AttackState.jumpAttack)
        {
            //rb.velocity = (transform.forward * 25);
            if (onGround)
            {
                nav.enabled = false;
                rb.AddForce(0, 1400, 0);
                rb.AddForce(-player.forward * 1600f);
                onGround = false;
            }
        }

        if(attackState == AttackState.HitPlayer)
        {
            nav.enabled = false;
            //this.transform.Rotate(Vector3.up * Time.deltaTime * 255);
            this.transform.localRotation *= Quaternion.Euler(new Vector3(0, 25, 0));
            Player.Damage(bulletDamage);

            rb.AddForce(0, 1400, 0);
            rb.AddForce(-player.forward * 1600f);
            attackState = AttackState.fly;
        }

        rb.velocity -= new Vector3(0, 65 * Time.deltaTime, 0);

        /*
        if(attackState == AttackState.findPlayer)
        {
            rb.mass = 1;
            nav.enabled = true;
            nav.SetDestination(player.transform.position);
            onGround = true;
        }

        else if(attackState == AttackState.jumpAttack)
        {
            //rb.velocity = (transform.forward * 25);
            if(rb.velocity.y <= 0 && onGround)
            {
                nav.enabled = false;
                rb.AddForce(-player.forward * 1600f);
                rb.AddForce(0, 900, 0);
                onGround = false;
                attackState = AttackState.fly;
            }

        }

        else if(attackState == AttackState.fly)
        {
            nav.enabled = false;
            this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position + (player.transform.forward * hitDistance), 55f * Time.deltaTime);

            if(DistanceBetweenPlayer() <= hitDistance)
            {
                //attackState = AttackState.HitPlayer;
            }
        }

        /*
        else if(attackState == AttackState.HitPlayer)
        {
            if (DistanceBetweenPlayer() <= hitDistance)
            {
                this.transform.Rotate(Vector3.right * 50 * Time.deltaTime);
            }
            else
            {
               // attackState = AttackState.jumpAttack;
            }
        }
        */
      //  print(attackState);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Bullet")
        {
            onGround = true;
            rb.velocity = Vector3.zero;
        }
        else if(collision.gameObject.tag == "Bullet")
        {
            if(attackState == AttackState.fly)
            {
                attackState = AttackState.findPlayer;
            }
        }
    }
    
    void Update () {
        /*
        if (this.GetComponent<Enemy>().health <= 0)
        {
            //this.GetComponent<HollowSnowman>().enabled = false;
           // return;
        }
        */

      //  if( > attackRange)
        
        if(DistanceBetweenPlayer() > attackRange && DistanceBetweenPlayer() > hitDistance)
        {
            attackState = AttackState.findPlayer;
        }
        else if(DistanceBetweenPlayer() < attackRange && DistanceBetweenPlayer() > hitDistance)
        {
            attackState = AttackState.jumpAttack;
        }
        else
        {
            attackState = AttackState.HitPlayer;
        }

        Attack();

        Color32 color = new Color32(255, 255, 255, 255);

        if(DistanceBetweenPlayer() < attackRange + 2)
        {
            //color = Color32.Lerp(color, new Color32(255, 0, 0, 255), 25 * Time.deltaTime);
            color = new Color32(255, 0, 0, 255);
        }


        glowingEyesAndMouth.SetColor("_EmissionColor", color);
	}

    // Nopeampi tapa saada pelaajan ja vihollisen välinen matka.
    private float DistanceBetweenPlayer()
    {
        return Vector3.Distance(player.position, this.transform.position);
    }
}
