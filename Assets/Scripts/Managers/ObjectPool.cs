using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public Queue<GameObject> CreatePool( GameObject prefab, int poolSize, Action<GameObject> onCreate)
    {
        Queue<GameObject> newQueue = new Queue<GameObject>();


        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            onCreate?.Invoke(obj);
            newQueue.Enqueue(obj);
        }

        return newQueue;
    }
}
