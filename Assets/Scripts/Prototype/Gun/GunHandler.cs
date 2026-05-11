using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunHandler : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private int gunDamage;
    
    [SerializeField] private float timeBetweenShots;
    
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Animator gunAnimator;

    public IFireStrategy fireStrategy;
    public IReloadStrategy reloadStrategy;
    
    private float nextFireTime;
    
    private void Awake()
    {
        fireStrategy = new HitscanFireStrategy(enemyLayerMask, Camera.main, gunDamage, gunAnimator);
        reloadStrategy = new NoReloadStrategy();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + timeBetweenShots;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Shoot()
    {
        fireStrategy.Fire();
    }

    private void Reload()
    {
        reloadStrategy.Reload();
    }
}
