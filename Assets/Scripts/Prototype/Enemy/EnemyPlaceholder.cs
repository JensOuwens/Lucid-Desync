using UnityEngine;

public class EnemyPlaceholder : MonoBehaviour, IDamagable
{
    private int health = 100;

    public void Hurt(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
