using System;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class DynamiteController : MonoBehaviour
{

    [SerializeField] private ParticleSystem vfxPrefab;
    private int timeToExplode = 5;
    private int distanceToKill = 2;
    public event Action<GameObject> OnDisabled;
    private Material dynamiteMaterial;


    private void Start()
    {
        dynamiteMaterial = GetComponent<Renderer>().material;
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
            PlayVFX(transform.position);
            OnDisabled?.Invoke(gameObject);
            gameObject.SetActive(false);
        });
    }
    public void PlayVFX(Vector3 position)
    {
        ParticleSystem vfx = Instantiate(vfxPrefab, position, Quaternion.identity);
        vfx.Play();

        Destroy(vfx.gameObject, vfx.main.duration + vfx.main.startLifetime.constantMax);
    }
}
