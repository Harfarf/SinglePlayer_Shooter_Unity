using UnityEngine;

public interface IDamageable
{
    bool IsAlive();
    int GetHealth();
    void SetDamage(int damage);
    void OnDie();


    Vector3 GetPosition();
}
