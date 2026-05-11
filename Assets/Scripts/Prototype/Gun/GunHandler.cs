using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunHandler : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayerMask;

    public IFireStrategy fireStrategy;
    public IReloadStrategy reloadStrategy;
    
    private void Awake()
    {
        fireStrategy = new HitscanFireStrategy(enemyLayerMask, Camera.main);
        reloadStrategy = new NoReloadStrategy();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(ShootRoutine());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            
        }
    }

    private IEnumerator ShootRoutine()
    {
        fireStrategy.Fire();
        yield return null;
    }

    private IEnumerator ReloadRoutine()
    {
        reloadStrategy.Reload();
        yield return null;
    }
}
