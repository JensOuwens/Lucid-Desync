using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    private float damage;
    private LayerMask layerMask;

    private Player player;
    private float rallyAmount;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(
        float damage,
        LayerMask layerMask,
        Player player,
        float rallyAmount)
    {
        this.damage = damage;
        this.layerMask = layerMask;

        this.player = player;
        this.rallyAmount = rallyAmount;
    }

    private void Update()
    {
        transform.position +=
            transform.forward * (bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only interact with valid targets
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            if (other.TryGetComponent<IDamagable>(out var damageable))
            {
                // Damage enemy
                damageable.Hurt(damage);

                // Rally only if PostDamageHealthRule is active
                if (player.GetHealthRule() is PostDamageHealthRule)
                {
                    player.Hurt(-rallyAmount);
                }
            }
        }

        Destroy(gameObject);
    }
}