using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float damage;
    private float lifetimeTimer;

    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float lifetime = 5f;

    private void OnEnable()
    {
        lifetimeTimer = lifetime;
    }

    public void Initialize(float damage)
    {
        this.damage = damage;
    }

    private void Update()
    {
        transform.position +=
            transform.forward * (projectileSpeed * Time.deltaTime);

        lifetimeTimer -= Time.deltaTime;

        if (lifetimeTimer <= 0f)
        {
            EnemyProjectilePool.Instance.Return(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var damageable))
        {
            damageable.Hurt(damage);
        }

        EnemyProjectilePool.Instance.Return(this);
    }
}