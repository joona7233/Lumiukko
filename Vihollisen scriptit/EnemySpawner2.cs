using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner2 : MonoBehaviour {

    //[SerializeField] private GameObject snowball;
    [SerializeField] private int enemyCount;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private float spawnRate;

    [SerializeField] private List<GameObject> snowballs = new List<GameObject>();

    [SerializeField] private Camera enemySpawnCamera;

    private Transform player;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(Spawn());
	}
	
    private IEnumerator Spawn()
    {
        float cameraOffset = 255f;

        while (true)
        {
            enemyCount = GameObject.FindGameObjectsWithTag("enemy").Length;

            //GameObject _snowball = Instantiate(snowball);

            if(enemyCount < maxEnemyCount)
            {
                enemySpawnCamera.transform.LookAt(player.transform.position + new Vector3(Random.Range(-cameraOffset, cameraOffset), Random.Range(-cameraOffset, cameraOffset), Random.Range(-cameraOffset, cameraOffset)));

                // Shoot snowball towrads player pos
                GameObject snowball = Instantiate(snowballs[Random.Range(0, snowballs.Count)]);
                Destroy(snowball, 3f);
                snowball.transform.position = enemySpawnCamera.transform.position;
                snowball.transform.LookAt(player.transform.position);
                Rigidbody rb = snowball.GetComponent<Rigidbody>();
                rb.AddForce(enemySpawnCamera.transform.forward * 12555f * Time.deltaTime, ForceMode.Impulse);

                yield return new WaitForSeconds(spawnRate);
            }

            yield return null;
        }
    }

	void Update () {
		


	}
}
