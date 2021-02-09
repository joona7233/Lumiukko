using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools = new List<Pool>();

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private Transform objpooL;


    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        objpooL = GameObject.FindGameObjectWithTag("gamemanager").transform;

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i=0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = objpooL;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject ReuseObject(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            print("Tag " + tag + " ei ole olemassa!");
            return null;
        }

        GameObject ots = poolDictionary[tag].Dequeue();

        ots.SetActive(true);
        ots.transform.parent = objpooL;
        poolDictionary[tag].Enqueue(ots);

        return ots;
    }
}
