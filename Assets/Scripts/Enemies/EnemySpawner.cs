using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    private Transform player;

    [SerializeField] private GameObject enemyPrefab;
    private Queue<GameObject> enemiesQueue;

    [SerializeField] private GameObject dynamitePrefab;
    private Queue<GameObject> dynamiteQueue;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InitEnemies();


        //Spawn enemies
        for (int i = 0; i < 7; i++)
        {
            StartCoroutine(SpawnEnemyFromQueue());
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
    private Vector3 GetEnemyRandomSpawnPos(int rangeX, int rangeY)
    {
        Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(rangeX, rangeY); //get a random direction and distance within the spawn ring
        Vector3 randomPos = player.position + new Vector3(offset.x, 0f, offset.y); //convert the offset into a world position around the player

        //find the nearest NavMesh point within 2 units
        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            return hit.position;

        return player.position + new Vector3(1f, 0f, 1f); //fallback
    }


    private IEnumerator SpawnEnemyFromQueue()
    {
        if (enemiesQueue.Count == 0) yield break;

        //reset enemy
        GameObject enemy = enemiesQueue.Dequeue();
        enemy.transform.localPosition = GetEnemyRandomSpawnPos(4,15);
        enemy.transform.localRotation = Quaternion.identity;
        enemy.SetActive(true);

        //track enemy
        while (enemy.activeSelf)
        {
            yield return null;
        }
        enemiesQueue.Enqueue(enemy);
        StartCoroutine(SpawnEnemyFromQueue());
    }
    private void SpawnDynamite()
    {
        if (dynamiteQueue.Count == 0) return;

        //spawn dynamite
        GameObject tnt = dynamiteQueue.Dequeue();
        tnt.transform.localPosition = GetEnemyRandomSpawnPos(1, 5);
        tnt.SetActive(true);

        //listen to gameobject disbaled to enqueue again
        tnt.GetComponent<DynamiteController>().OnDisabled += (obj) =>
        {
            dynamiteQueue.Enqueue(obj);
        };
    }

}
