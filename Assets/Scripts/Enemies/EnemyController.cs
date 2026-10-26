using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    private GameObject player;
    private int health = 20;


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
        this.transform.LookAt(player.transform.position);

        //if close enough to the player then kill him
        if (Vector3.Distance(player.transform.position, transform.position) < 1.5f)
        {
            player.GetComponent<IDamageable>().OnDie();
        }
    }


    #region IDamageable
    public void SetDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0) OnDie();
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
        this.gameObject.SetActive(false);
    }
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    #endregion
}
