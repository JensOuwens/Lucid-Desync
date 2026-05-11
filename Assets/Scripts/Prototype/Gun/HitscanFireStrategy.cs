using UnityEngine;

public class HitscanFireStrategy : IFireStrategy
{
    private LayerMask enemyLayerMask;
    Camera mainCamera;
    
    public HitscanFireStrategy(LayerMask mask, Camera camera)
    {
        enemyLayerMask = mask;
        this.mainCamera = camera;
    }
    
    public void Fire()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (!Physics.Raycast(ray , out hit, 1000)) return;
        
        if ((enemyLayerMask & (1 << hit.transform.gameObject.layer)) != 0)
        {
            Debug.Log("Enemy hit");
        }
    }
}
