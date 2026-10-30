using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform player;

    [SerializeField] private GameObject enemyPrefab;
    private Queue<GameObject> enemiesQueue;

    [SerializeField] private GameObject dynamitePrefab;
    private Queue<GameObject> dynamiteQueue;

    private int enemiesToSpawn = 7;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InitEnemies();


        //Spawn enemies
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemyFromQueue();
        }
        //Spawn dynamites
        InvokeRepeating(nameof(SpawnDynamite), 2f, 3f);
    }




    private void InitEnemies()
    {
        dynamiteQueue = ObjectPool.Instance.CreatePool(dynamitePrefab, 5, null);
        enemiesQueue = ObjectPool.Instance.CreatePool(enemyPrefab, 8, (newEnemy) =>
        {
            newEnemy.GetComponent<EnemyController>().Initialize();
        });
    }


    private void SpawnEnemyFromQueue()
    {
        if (enemiesQueue.Count == 0) return;

        //reset enemy
        GameObject enemy = enemiesQueue.Dequeue();
        enemy.transform.localPosition = SceneHandler.Instance.GetRandomPointInNavMesh(player, 4, 15);
        enemy.transform.localRotation = Quaternion.identity;
        enemy.SetActive(true);

        //track enemy death
        enemy.GetComponent<EnemyController>().OnDead += () =>
        {
            enemiesQueue.Enqueue(enemy);
            SpawnEnemyFromQueue();
        };
        
    }

    private void SpawnDynamite()
    {
        if (dynamiteQueue.Count == 0) return;

        //spawn dynamite
        GameObject tnt = dynamiteQueue.Dequeue();
        tnt.transform.localPosition = SceneHandler.Instance.GetRandomPointInNavMesh(player, 2, 5);
        tnt.SetActive(true);

        //listen to gameobject disbaled to enqueue again
        tnt.GetComponent<DynamiteController>().OnDisabled += () =>
        {
            dynamiteQueue.Enqueue(tnt);
        }; //TODO:? unsubscribe???????????????????????????????????????''
    }

}
