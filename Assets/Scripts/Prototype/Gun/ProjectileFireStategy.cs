using UnityEngine;

public class ProjectileFireStategy : IFireStrategy
{
    private LayerMask enemyLayerMask;
    private Camera mainCamera;
    private int gunDamage;
    private Animator gunAnimator;
    private GameObject projectilePrefab;

    public ProjectileFireStategy(
        LayerMask mask,
        Camera camera,
        int gunDamage,
        Animator gunAnimator,
        GameObject projectilePrefab)
    {
        enemyLayerMask = mask;
        this.mainCamera = camera;
        this.gunDamage = gunDamage;
        this.gunAnimator = gunAnimator;
        this.projectilePrefab = projectilePrefab;
    }

    public void Fire()
    {
        gunAnimator.SetTrigger("Fire");

        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * 1.5f;

        GameObject projectile = Object.Instantiate(
            projectilePrefab,
            spawnPosition,
            mainCamera.transform.rotation
        );

        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.linearVelocity = mainCamera.transform.forward * 15f;

        Projectile projectileScript = projectile.GetComponent<Projectile>();

        projectileScript.SetDamage(gunDamage);

        Object.Destroy(projectile, 5f);
    }
}