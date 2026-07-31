using UnityEngine;

/// <summary>
/// TODO: add a firepoint where the bullet comes from later
/// </summary>
public class ProjectileGunRule : IWeaponRule
{
    private float damage;
    private float fireRate;
    private Camera playerCamera;
    private GameObject bulletPrefab;
    private LayerMask enemyLayerMask;
    
    private float projectileSpawnOffset = 1f;
    
    private float nextFireTime;

    public ProjectileGunRule(float damage, float fireRate, Camera playerCamera, GameObject bulletPrefab, LayerMask enemyLayerMask)
    {
        this.damage = damage;
        this.fireRate = fireRate;
        this.playerCamera = playerCamera;
        this.bulletPrefab = bulletPrefab;
        this.enemyLayerMask = enemyLayerMask;
    }
    public bool Press()
    {
        // Check cooldown first
        if (Time.time < nextFireTime)
            return false;

        nextFireTime = Time.time + fireRate;

        //fire projectile
        GameObject bullet = Object.Instantiate(bulletPrefab, playerCamera.transform.position + playerCamera.transform.forward * projectileSpawnOffset, playerCamera.transform.rotation);
        
        bullet.GetComponent<ProjectileBullet>().Initialize(damage, enemyLayerMask);
        
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
