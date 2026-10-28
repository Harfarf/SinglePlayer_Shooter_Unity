using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    private GameObject player;
    private int health = 20;

    [SerializeField] private GameObject vfxPrefab;
    private Queue<GameObject> vfxQueue;


    void Start()
    {
        vfxQueue = ObjectPool.Instance.CreatePool(vfxPrefab, 2, null);
    }
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        ChasePlayer();
    }



    private void ChasePlayer()
    {
        GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        transform.LookAt(player.transform.position);

        //if close enough to the player then kill him
        if (Vector3.Distance(player.transform.position, transform.position) < 1.5f)
        {
            player.GetComponent<IDamageable>().OnDie();
        }
    }



    #region IDamageable
    public void SetDamage(int damageAmount, GameObject attacker)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            if (attacker && attacker.GetComponent<PlayerController>())
            {
                attacker.GetComponent<PlayerController>().RegisterKill();
            }
            OnDie();
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public int GetHealth()
    {
        return health;
    }
    public void OnDie()
    {
        ObjectPool.Instance.PlayVFXFromPool(vfxQueue, transform.position);
        gameObject.SetActive(false);
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    #endregion
}
