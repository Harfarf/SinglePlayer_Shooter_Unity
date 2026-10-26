using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;

    private Queue <GameObject> bulletsQueue;
    private int maxBullets = 20;
    private float bulletSpeed = 9f;


    private void Start()
    {
        InitBullets();
    }
    private void Update()
    {
        if (InputManager.Instance.GetInput(InputCommand.Attack)) BulletAttack();
    }




    private void InitBullets()
    {
        bulletsQueue = ObjectPool.Instance.CreatePool(bulletPrefab, maxBullets, (newBullet) =>
        {
            newBullet.GetComponent<Bullet>().Initialize(gameObject);
        });
    }

    private void BulletAttack()
    {
        StartCoroutine("SpawnBulletFromQueue");
    }
    private IEnumerator SpawnBulletFromQueue()
    {
        if (bulletsQueue.Count == 0) yield break;

        //reset bullet
        GameObject bullet = bulletsQueue.Dequeue();
        bullet.transform.localPosition = bulletSpawnPos.position;
        bullet.transform.localRotation = bulletSpawnPos.rotation;
        Vector3 dir = transform.forward;
        bullet.SetActive(true);

        //move bullet
        while (bullet.activeSelf)
        {
            bullet.GetComponent<Rigidbody>().linearVelocity = dir.normalized * bulletSpeed; //units per second - consistent between devices
            yield return null;
        }
        bulletsQueue.Enqueue(bullet);
    }

}
