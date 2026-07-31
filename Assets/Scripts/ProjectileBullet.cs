using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    private float damage;
    private LayerMask layerMask;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float lifetime;
    
    private void Start()
    {
        Destroy(this.gameObject, lifetime);
    }
    
    public void Initialize(float damage, LayerMask layerMask)
    {
        this.damage = damage;
        this.layerMask = layerMask;
    }
    
    private void Update()
    {
        transform.position += transform.forward * (bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only damage valid targets
        if (((1 << other.gameObject.layer) & layerMask.value) != 0)
        {
            if (other.TryGetComponent<IDamagable>(out var damageable))
            {
                damageable.Hurt(damage);
            }
        }
        
        Destroy(gameObject);
    }
}
