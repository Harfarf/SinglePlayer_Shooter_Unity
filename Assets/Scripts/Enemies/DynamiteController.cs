using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamiteController : MonoBehaviour
{

    private int timeToExplode = 5;
    private int distanceToKill = 2;
    private Material dynamiteMaterial;

    [SerializeField] private GameObject vfxPrefab;
    private Queue<GameObject> vfxQueue;

    public event Action OnDisabled;


    private void Start()
    {
        dynamiteMaterial = GetComponent<Renderer>().material;
        vfxQueue = ObjectPool.Instance.CreatePool(vfxPrefab, 2, null);
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
        dynamiteMaterial.DOColor(Color.red, 0.5f).SetLoops(6, LoopType.Yoyo).OnComplete(() =>
        {
            CheckDistanceToKill();
            ObjectPool.Instance.PlayVFXFromPool(vfxQueue, transform.position);
            OnDisabled?.Invoke();
            gameObject.SetActive(false);
        });
    }

}
