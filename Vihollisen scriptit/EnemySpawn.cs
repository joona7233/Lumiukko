using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] GameObject[] enemies;

    void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag == "Player")

       
        InvokeRepeating("SpawnEnemyRandom", 1, 4);
        // gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        CancelInvoke("SpawnEnemyRandom");
    }


    void SpawnEnemyRandom()
    {
        if (Enemies.GetEnemyCount() >= Enemies.Instance.maxEnemyCoumt)
            return;

        Transform sp = spawnPositions[Random.Range(0, spawnPositions.Length)];
        //GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Length)],sp.position, sp.rotation);
        // Joona
        GameObject enemy = Instantiate(Enemies.Instance.enemies[Random.Range(0, Enemies.Instance.enemies.Count)]);
        enemy.tag = "enemy";
    }
}
