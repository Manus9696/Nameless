﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    
    [System.Serializable]
    public class Pool
    {
        public string tag; 
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

	void Start () {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i=0; i<pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }

        foreach (Pool pool in pools)
        {
            Debug.Log(pool.tag + "#");
        }


    }

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Should never happen");
            return null;
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objToSpawn.GetComponent<IPooledObject>();

        if(pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }

}
