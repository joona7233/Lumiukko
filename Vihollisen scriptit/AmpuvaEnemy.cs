using UnityEngine;
using UnityEngine.AI;

public class AmpuvaEnemy : EnemyBehaviour
{
    [Header("")]
    
    
    public float attackDistance; // Etäisyys, jolla aloitetaan hyökkäys kohti playeria
    public float enemyFireRate; // Kuinka nopeaan tahtiin enemy ampuu
    //[Range(0.0f, 1.0f)]
    //public float accuracy = 0.5f; // Osumatarkkuuden liukusäädin inspectoriin
    public ParticleSystem enemynPiippuefekti; // Laita inspectorissa partikkeliefekti
    public AudioClip ampumisSound; // Laita inspectorissa audio clippi

    int enemyAttackDamage = 50; // Enemyn yhden osuman damage playerille
    float nextFire; // Määrittää välin ampumiselle
    bool chanceToShoot = false;
    Animator anim;
    
    void Awake()
    {
        
       
        IdentifyPlayer(); // Tunnistetaan pelaaja       
  
        anim = GetComponent<Animator>();
        
    }

    void Update()
    {
    
        ReachPlayer(); // kulkeminen ja hyökkääminen pelaajaa kohti
        Invoke("CheckChanceToAttack", 2); // Invokella laitetaan aikaraja, ettei heti spawnauksen jälkeen rupea ampumaan

        if (chanceToShoot) // Jos ampumaetäisyys hyvä, tämä value on true
        {
            AttackPlayer();
        }
    }

    #region Lisätyt metodit


    void CheckChanceToAttack() // Hyökkää kohti pelaajaa, jos välimatka on oikea
    {
        
        if (distanceBetween < attackDistance) // Jos etäisyys playeriin on oikea, enemyy tekee hyökkäyksen
        {
            if (Time.time > nextFire) // Ampumiskertojen aikavälin laskemiseksi
            {
                chanceToShoot = true; // Mahdollisuus ampua      
            }
        }
    }

 

    protected override bool SeePlayer() // Tarkastetaan onko enemyllä näköyhteys pelaajaan
    {
        RaycastHit hit;
        Ray rayDirection = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(rayDirection, out hit))
        {
            if (hit.collider.tag == "PlayerLAP")
            {
                // enemy can see the player!
                return true;
            }
        }

        return false;
    }

    protected override void AttackPlayer() // Ylikirjoitetaan kantaluokka Enemyn metodi
    {
        #region toni
        /*
        nextFire = Time.time + enemyFireRate; // enemyFireRate määrittää ampumisyritysten aikavälin
        float tryToHit = Random.Range(0.0f, 1.0f); // Luodaan arvonta enemyn osumistarkkuutta varten
        bool playerGotHit = tryToHit > 1.0f - accuracy; // Tarkistetaan onko arvottu numero isompi kuin inspectorissa määritetty osumatarkkuus
        enemynPiippuefekti.Play(); // Näytetään enemyn ampumaefekti
        AudioSource.PlayClipAtPoint(ampumisSound, transform.position); // Soitetaan ampumisen ääni

      
        if (playerGotHit == true) // jos arvottu arvo on isompi..
        {
            InflictPlayerDamage(enemyAttackDamage); // Kutsutaan  metodia, joka suorittaa HP vähennyksen playerilta
            playerGotHit = false; // Asetaan osuma-arvo takaisin epätodeksi
        }
        chanceToShoot = false;  // Takaisin oletusarvoksi, koska enemy suorittaa CheckChanceToKill uudestaan
        */
        #endregion

        #region Joona
        /*
        nextFire = Time.time + enemyFireRate; // enemyFireRate määrittää ampumisyritysten aikavälin
        float tryToHit = Random.Range(0.0f, 1.0f); // Luodaan arvonta enemyn osumistarkkuutta varten
        bool playerGotHit = tryToHit > 1.0f - accuracy; // Tarkistetaan onko arvottu numero isompi kuin inspectorissa määritetty osumatarkkuus
        enemynPiippuefekti.Play(); // Näytetään enemyn ampumaefekti
        AudioSource.PlayClipAtPoint(ampumisSound, transform.position); // Soitetaan ampumisen ääni

        if (playerGotHit == true) // jos arvottu arvo on isompi..
        {
            InflictPlayerDamage(enemyAttackDamage); // Kutsutaan  metodia, joka suorittaa HP vähennyksen playerilta
            playerGotHit = false; // Asetaan osuma-arvo takaisin epätodeksi
        }
        chanceToShoot = false;  // Takaisin oletusarvoksi, koska enemy suorittaa CheckChanceToKill uudestaan


        if (SeePlayer() && InRange())
        {
            float r = Random.Range(0f, 1f);
            GameObject p;
            if (r < accuracy)
            {
                p = ShootRay(0, 15, false);
            }
            else
                p = ShootRay(bulletSpread, 25, false);

            if (p.tag == "Player")
            {
                Player.Damage(bulletDamage);
                print("hit");
            }
        }
        */
        #endregion



        #region Tonin uudempi
        //Toimiva

        nextFire = Time.time + enemyFireRate; // enemyFireRate määrittää ampumisyritysten aikavälin
        float tryToHit = Random.Range(0.0f, 1.0f); // Luodaan arvonta enemyn osumistarkkuutta varten
        bool playerGotHit = tryToHit > 1.0f - accuracy; // Tarkistetaan onko arvottu numero isompi kuin inspectorissa määritetty osumatarkkuus
        enemynPiippuefekti.Play(); // Näytetään enemyn ampumaefekti
        AudioSource.PlayClipAtPoint(ampumisSound, transform.position); // Soitetaan ampumisen ääni
        GameObject p;
        Debug.Log("Enemy ampuu");

        if (playerGotHit && InRange() && SeePlayer()) // jos arvottu arvo on isompi..
        {
            Debug.Log("Enemy osuu");
            p = ShootRay(bulletSpread, 25, false);
            enemyAttackDamage += bulletDamage;
            InflictPlayerDamage(enemyAttackDamage); // Kutsutaan  metodia, joka suorittaa HP vähennyksen playerilta
            playerGotHit = false; // Asetaan osuma-arvo takaisin epätodeksi
        }
        else
        {
            p = ShootRay(0, 15, false);
        }
        chanceToShoot = false;  // Takaisin oletusarvoksi, koska enemy suorittaa CheckChanceToKill uudestaan
        anim.SetBool("IsShooting", false);

        #endregion 
    }

    #endregion
}