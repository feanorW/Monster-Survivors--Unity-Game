using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    private List<GameObject> pool;

    private void Start()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject newObj = Instantiate(prefab, transform);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return CreateNewObject(); // If no inactive object found, create a new one
    }
}