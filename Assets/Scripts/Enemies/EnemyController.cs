using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    private enum EnemyState { Patrol, Chase }

    private GameObject player;
    private NavMeshAgent Nagent;
    [SerializeField] private Renderer renderMesh;

    [SerializeField] private List<Enemy_SO> enemyTypes = new List<Enemy_SO>();

    [SerializeField] private GameObject vfxPrefab;
    private Queue<GameObject> vfxQueue;

    private int health;
    private float chaseDistance = 10f;
    private float killDistance = 0.5f;

    public event Action OnDead;




    void Start()
    {
        vfxQueue = ObjectPool.Instance.CreatePool(vfxPrefab, 2, null, null);
        Nagent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        StateMachine();
    }



    public void Initialize()
    {
        Enemy_SO enemyDescription = enemyTypes[UnityEngine.Random.Range(0, enemyTypes.Count)]; //random config per enemy
        renderMesh.material.color = enemyDescription.enemyColor;
        renderMesh.material.SetColor("_EmissionColor", enemyDescription.enemyColor);
        health = enemyDescription.maxHealth;
    }
    private void StateMachine()
    {
        //state machine
        float distanceWithPlayer = Vector3.Distance(player.transform.position, transform.position);
        EnemyState state = distanceWithPlayer < chaseDistance ? EnemyState.Chase : EnemyState.Patrol;
        switch (state)
        {
            case EnemyState.Patrol:
                if (!Nagent.pathPending && (!Nagent.hasPath || Nagent.remainingDistance <= 0.5f))
                {
                    Vector3 patrolPos = GeneralFunctions.Instance.GetRandomPointInNavMesh(transform, 5, 10);
                    Nagent.SetDestination(patrolPos);
                    transform.LookAt(patrolPos);
                }
            break;
            case EnemyState.Chase:
                Nagent.SetDestination(player.transform.position);
                transform.LookAt(player.transform.position);
            break;
        }

        //if close enough to the player then kill him
        if (distanceWithPlayer < killDistance)
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
        OnDead.Invoke();
        gameObject.SetActive(false);
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    #endregion
}
