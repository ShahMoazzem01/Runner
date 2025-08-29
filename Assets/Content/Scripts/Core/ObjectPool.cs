using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    void Awake()
    {
        // pre instantiate objects
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        //extend pool if its empty
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }


}
