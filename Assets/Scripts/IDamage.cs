using UnityEngine;

public interface IDamage {
    void ApplyDamage(int damage, out bool killed);
    void ApplyDamage(int damage, out bool killed, out GameObject obj);
    void Die();
    void Heal(int h);
}
