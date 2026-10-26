using System;
using System.Linq;
using UnityEngine;

public class DynamiteController : MonoBehaviour
{

    private GameObject player;
    private int timeToExplode = 10;
    private int distanceToKill = 2;
    public event Action<GameObject> OnDisabled;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnEnable()
    {
        Invoke(nameof(Explode), timeToExplode);
    }





    private void CheckDistanceToKill()
    {
        IDamageable[] damageables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDamageable>().ToArray();

        foreach (IDamageable damageable in damageables)
        {
            int distance = (int)Vector3.Distance(damageable.GetPosition(), transform.position);

            if (distance <= distanceToKill) //kills both player or enemies
            {
                damageable.OnDie();
            }
        }

    }

    private void Explode()
    {
        CheckDistanceToKill();
        OnDisabled?.Invoke(gameObject);
        gameObject.SetActive(false);
    }
}
