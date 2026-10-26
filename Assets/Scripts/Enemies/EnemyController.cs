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
        ChasePlayer(); //en mauqina de estados mejor
    }



    private void ChasePlayer()
    {
        GetComponent<NavMeshAgent>().SetDestination(player.transform.position);
        this.transform.LookAt(player.transform.position);
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
