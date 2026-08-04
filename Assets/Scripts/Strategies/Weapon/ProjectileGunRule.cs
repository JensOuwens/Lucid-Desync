using UnityEngine;

/// <summary>
/// TODO: add a firepoint where the bullet comes from later
/// </summary>
public class ProjectileGunRule : IWeaponRule
{
    private float damage;
    private float fireRate;
    private float rallyAmount;

    private Camera playerCamera;
    private GameObject bulletPrefab;
    private LayerMask enemyLayerMask;

    private Player player;

    private float projectileSpawnOffset = 1f;
    private float nextFireTime;

    public ProjectileGunRule(
        float damage,
        float fireRate,
        Camera playerCamera,
        GameObject bulletPrefab,
        LayerMask enemyLayerMask,
        Player player,
        float rallyAmount)
    {
        this.damage = damage;
        this.fireRate = fireRate;
        this.playerCamera = playerCamera;
        this.bulletPrefab = bulletPrefab;
        this.enemyLayerMask = enemyLayerMask;

        this.player = player;
        this.rallyAmount = rallyAmount;
    }

    public bool Press()
    {
        // Check cooldown
        if (Time.time < nextFireTime)
            return false;

        nextFireTime = Time.time + fireRate;

        // Fire projectile
        GameObject bullet = Object.Instantiate(
            bulletPrefab,
            playerCamera.transform.position +
            playerCamera.transform.forward * projectileSpawnOffset,
            playerCamera.transform.rotation
        );

        bullet.GetComponent<ProjectileBullet>().Initialize(
            damage,
            enemyLayerMask,
            player,
            rallyAmount
        );

        return true;
    }

    public void Hold()
    {
        // Do nothing
    }

    public bool Release()
    {
        return false;
    }
}