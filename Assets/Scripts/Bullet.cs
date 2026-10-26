using UnityEngine;
using System.Threading.Tasks;

public class Bullet : MonoBehaviour
{
    private int lifeInSeconds = 5;
    private GameObject bulletOwner;


    private async void OnEnable()
    {
        try
        {
            await LifeWindow(); //thread
        }
        catch (System.OperationCanceledException)
        {
            //operation cancelled
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (bulletOwner && bulletOwner==other.gameObject)
        {
            return; //ignore owner
        }
        else if (other.GetComponent<IDamageable>() != null) //player & enemy
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable.SetDamage(10);
            this.gameObject.SetActive(false);
        }

    }



    public void Initialize(GameObject owner)
    {
        bulletOwner = owner;
    }

    private async Task LifeWindow()
    {
        await Awaitable.WaitForSecondsAsync( lifeInSeconds, destroyCancellationToken );

        gameObject.SetActive(false);
    }
}
