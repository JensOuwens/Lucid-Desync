using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.Hurt(damage);
        }

        Destroy(gameObject);
    }
}