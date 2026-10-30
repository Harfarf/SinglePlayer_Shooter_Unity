using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public Queue<GameObject> CreatePool( GameObject prefab, int poolSize, Action<GameObject> onCreate, Action<GameObject> onDisabled)
    {
        Queue<GameObject> newQueue = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            onCreate?.Invoke(obj);
            newQueue.Enqueue(obj);
            onDisabled?.Invoke(obj);
        }

        return newQueue;
    }


    public void PlayVFXFromPool(Queue<GameObject> vfxQueue, Vector3 position)
    {
        StartCoroutine(PlayVFXFromPoolCoroutine(vfxQueue, position));
    }
    private IEnumerator PlayVFXFromPoolCoroutine(Queue<GameObject> vfxQueue, Vector3 position)
    {
        //reset vfx
        GameObject vfx = vfxQueue.Dequeue();
        vfx.transform.localPosition = position;
        vfx.SetActive(true);

        //play vfx
        var particles = vfx.GetComponent<ParticleSystem>();
        particles.Play();

        yield return new WaitForSeconds(particles.main.duration + particles.main.startLifetime.constantMax);

        //enqueue again
        vfx.SetActive(false);
        vfxQueue.Enqueue(vfx);

        yield return null;
    }
}
