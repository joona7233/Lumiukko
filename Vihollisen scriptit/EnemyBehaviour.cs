using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour {

    protected float distanceBetween;
    protected Transform player;
    protected NavMeshAgent nav;

    [Header("Ampumisen asetukset...")]
    [SerializeField] [Range(0, 1)] protected float accuracy;
    [SerializeField] protected GameObject bulletHitParticle;
    [SerializeField] protected GameObject particleFire;

    [Header("")]
    [SerializeField] protected int bulletDamage;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float maxRange;

    [HideInInspector] protected float currentCooldown;

    [Header("Visual")]
    [SerializeField][Range(0, 4)] protected float bulletSpread;
    //[SerializeField] protected GameObject bulletPrefab;

    [Header("Other")]
    [SerializeField] protected Transform shootingPoint;

    void Awake()
    {
        //player = GameObject.FindGameObjectWithTag("PlayerLAP").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        nav = GetComponent<NavMeshAgent>();
    }
    
    protected void IdentifyPlayer() // Awake() metodissa, tunnistaa "Player"in 
    {
        player = GameObject.FindWithTag("PlayerLAP").transform;

        nav = GetComponent<NavMeshAgent>();

       // shootingPoint = Utility.FindTransformFromChildren("ShootingPoint", this.transform);
    }

    protected virtual void ReachPlayer() // Update metodissa, kutsuttaessa seuraa kohdetta player
    {
        nav.destination = (player.transform.position); // Navigoi kohti pelaajaa

        if (player != null)
        {
            transform.LookAt(player); // Säilyttää katseen pelaajassa
        }

        distanceBetween = Vector3.Distance(GameObject.FindWithTag("PlayerLAP").transform.position, this.transform.position);
        // Playerin ja enemyn etäisyys
    }

    protected virtual void AttackPlayer()
    {
        
    }

    protected void InflictPlayerDamage(int enemyAttackDamage) // Suoritetaan HP vähennys Playerilta
    {
        Player.Damage(enemyAttackDamage);
        // Kutsutaan Player-luokan Damage metodia ja lähetetään inspectorissa määritetty enemyn tuhovoima
    }

    /*
    protected void SpawnBullets(int amount)
    {
        for(int i=0; i < amount; i++)
        {
            float spread = bulletSpread;
            Vector3 rPos = new Vector3(-Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread));
            Vector3 defaultRotation = shootingPoint.transform.rotation.eulerAngles;
            Vector3 finalRotation = defaultRotation + rPos;
            
            GameObject bullet = Instantiate(bulletPrefab);
            //bullet.GetComponent<Bullet>().damage = weaponStats.damage;
            bullet.transform.position = shootingPoint.transform.position;
            //bullet.transform.rotation = Quaternion.Euler(finalRotation);
            bullet.transform.LookAt(player);

            bullet.GetComponent<EnemyBullet>().damage = bulletDamage;
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 160;
            Destroy(bullet, 5f);
            
        }
    }
    */

    protected GameObject ShootRay(float scaleLimit = .32f, float z = 15, bool instantiateGunFire = true, bool drawHelpLine = false)
    {

        //  Try this one first, before using the second one
        //  The Ray-hits will form a ring
        float randomRadius = scaleLimit; 
        //  The Ray-hits will be in a circular area
        //   float randomRadius = Random.Range(0, scaleLimit);

        float randomAngle = Random.Range(0, 2 * Mathf.PI);

        //Calculating the raycast direction
        Vector3 direction = new Vector3(
            randomRadius * Mathf.Cos(randomAngle),
            randomRadius * Mathf.Sin(randomAngle),
            z
        );

        //Make the direction match the transform
        //It is like converting the Vector3.forward to transform.forward
        direction = transform.TransformDirection(direction.normalized);

        //Raycast and debug
        Ray r = new Ray(transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            if(drawHelpLine)
                Debug.DrawLine(transform.position, hit.point);

            GameObject bhp = Instantiate(bulletHitParticle);
            bhp.transform.position = hit.point;
            Destroy(bhp, 1f);

            if (instantiateGunFire)
            {
                GameObject shootParticle = Instantiate(particleFire);
                //shootParticle.transform.position = hit.point;
                shootParticle.transform.parent = shootingPoint.transform;
                shootParticle.transform.position = shootingPoint.transform.position;
                shootParticle.transform.localScale = new Vector3(1, 1, 1);
                shootParticle.transform.LookAt(hit.point);
                Destroy(shootParticle, 1f);

            }

            return hit.collider.gameObject;
        } 

        return null;
    }

    protected void JoonanAttackPlayer(bool InstantiateGunFire)
    {
        float r = Random.Range(0f, 1f);
        GameObject p;

        if (accuracy >= r)
        {
            p = ShootRay(0f, 15f, InstantiateGunFire);

            if (p.gameObject.tag == "Player")
            {
                Player.Damage(bulletDamage);
            }
        }
        else
        {
            p = ShootRay(bulletSpread, 15f, InstantiateGunFire);
        }
       
    }

    protected virtual bool SeePlayer()
    {
        RaycastHit hit;
        var rayDirection = player.position - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if (hit.transform.tag == "Player" && Player.alive)
            {
                // enemy can see the player!
                return true;
            }
        }

        return false;
    }

    protected bool InRange()
    {
        return Vector3.Distance(player.transform.position, this.transform.position) <= maxRange;
    }

    protected void IncreaseAccuracyOverTime(float amount)
    {
        if(accuracy < 1f)
        {
            accuracy += amount * Time.deltaTime;
            if (accuracy > 1)
                accuracy = 1;
        }
    }

    protected void DecreaseAccuracyOverTime(float amount)
    {
        if(accuracy > 0)
        {
            accuracy -= amount * Time.deltaTime;
            if (accuracy < 0)
                accuracy = 0;
        }
    }
    

}

