using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour {

    public int limitEnemyCount;
    public int maxEnemyCoumt;

    public List<GameObject> enemies = new List<GameObject>();

    public static Enemies Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnRandomEnemy(Vector3 pos)
    {
        if(limitEnemyCount <= GameObject.FindGameObjectsWithTag("enemy").Length)
            return;

        if (!Player.alive)
            return;

        GameObject enemy = Instantiate(enemies[Random.Range(0, enemies.Count)]);
        enemy.transform.localPosition = pos + new Vector3(0, 2f, 0);
        enemy.tag = "enemy";
    }

    public static int GetEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("enemy").Length;
    }
}
