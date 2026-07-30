using UnityEngine;

public class HitscanGunRule : IWeaponRule
{
    private LayerMask enemyLayerMask;
    private float damage;
    private float fireRate;
    private GameObject playerGameObject;
    
    private float nextFireTime;

    public HitscanGunRule(LayerMask layerMask, float damage, float fireRate, GameObject player)
    {
        enemyLayerMask =  layerMask;
        this.damage = damage;
        this.fireRate = fireRate;
        playerGameObject = player;
    }
    
    public bool Press()
    {
        // Check cooldown first
        if (Time.time < nextFireTime)
            return false;

        nextFireTime = Time.time + fireRate;

        // Raycast
        if (!Physics.Raycast(
                playerGameObject.transform.position,
                playerGameObject.transform.forward,
                out RaycastHit hitInfo,
                100))
            return true;

        // Check if hit is in layermask
        if (((1 << hitInfo.collider.gameObject.layer) & enemyLayerMask.value) == 0)
            return true;
        

        // Damage
        if (hitInfo.collider.TryGetComponent<IDamagable>(out var damageable))
        {
            damageable.Hurt(damage);
        }
        
        return true;
    }

    public void Hold()
    {
        //do nothing
    }

    public bool Release()
    {
        return false;
    }
}
