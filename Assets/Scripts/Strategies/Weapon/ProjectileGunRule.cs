using UnityEngine;

public class ProjectileGunRule : IWeaponRule
{
    private float fireRate;
    private GameObject playerGameObject;
    
    private GameObject bulletPrefab;
    
    private float nextFireTime;
    public bool Press()
    {
        // Check cooldown first
        if (Time.time < nextFireTime)
            return false;

        nextFireTime = Time.time + fireRate;

        //fire projectile
        
        
        // Raycast
        //if (!Physics.Raycast(
            //      playerGameObject.transform.position,
            //      playerGameObject.transform.forward,
            //      out RaycastHit hitInfo,
            //      100))
            // return true;

        // Check if hit is in layermask
        // if (((1 << hitInfo.collider.gameObject.layer) & enemyLayerMask.value) == 0)
        //     return true;
        //
        //
        // // Damage
        // if (hitInfo.collider.TryGetComponent<IDamagable>(out var damageable))
        // {
        //     damageable.Hurt(damage);
        // }
        
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
