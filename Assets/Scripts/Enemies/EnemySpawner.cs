using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.STP;

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
        enemiesQueue = ObjectPool.Instance.CreatePool(enemyPrefab, 8, null);
        dynamiteQueue = ObjectPool.Instance.CreatePool(dynamitePrefab, 5, null);
    }
    private Vector3 GetEnemyRandomSpawnPos(int rangeX, int rangeY)
    {
        float xPos = Random.Range(player.position.x + rangeX, player.position.x + rangeY) * (Random.value < 0.5f ? 1 : -1);
        float zPos = Random.Range(player.position.z + rangeX, player.position.z + rangeY) * (Random.value < 0.5f ? 1 : -1);
        Vector3 randomPos = new Vector3(xPos, 0, zPos);

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 10, NavMesh.AllAreas)) //make sure it spwans inside the navmesh
            return hit.position;

        return randomPos;
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
