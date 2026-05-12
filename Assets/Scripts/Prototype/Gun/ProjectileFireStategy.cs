using UnityEngine;

/// <summary>
/// TODO: add projectile stuff
/// </summary>
public class ProjectileFireStategy : IFireStrategy
{
    private LayerMask enemyLayerMask;
    private Camera mainCamera;
    private int gunDamage;
    private Animator gunAnimator;
    private GameObject projectilePrefab;
    
    public ProjectileFireStategy(LayerMask mask, Camera camera, int gunDamage, Animator gunAnimator, GameObject projectilePrefab)
    {
        enemyLayerMask = mask;
        this.mainCamera = camera;
        this.gunDamage = gunDamage;
        this.gunAnimator = gunAnimator;
        this.projectilePrefab = projectilePrefab;
    }
    
    public void Fire()
    {
        
    }
}
