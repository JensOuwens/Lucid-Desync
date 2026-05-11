using UnityEngine;

public class HitscanFireStrategy : IFireStrategy
{
    private LayerMask enemyLayerMask;
    private Camera mainCamera;
    private int gunDamage;
    private Animator gunAnimator;
    
    public HitscanFireStrategy(LayerMask mask, Camera camera, int gunDamage, Animator gunAnimator)
    {
        enemyLayerMask = mask;
        this.mainCamera = camera;
        this.gunDamage = gunDamage;
        this.gunAnimator = gunAnimator;
    }
    
    public void Fire()
    {
        gunAnimator.SetTrigger("Fire");
        
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (!Physics.Raycast(ray , out hit, 1000)) return;
        
        if ((enemyLayerMask & (1 << hit.transform.gameObject.layer)) != 0)
        {
            hit.transform.gameObject.GetComponent<IDamagable>().Hurt(gunDamage);
        }
    }
}
