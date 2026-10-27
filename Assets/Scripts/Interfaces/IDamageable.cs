using UnityEngine;

public interface IDamageable
{
    bool IsAlive();
    int GetHealth();
    void SetDamage(int damage, GameObject attacker);
    void OnDie();


    Vector3 GetPosition();
}
