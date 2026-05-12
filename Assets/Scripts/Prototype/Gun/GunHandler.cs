using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GunHandler : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private int gunDamage;
    
    [SerializeField] private float timeBetweenShots;
    
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Animator gunAnimator;

    [SerializeField] private int maxAmmo;
    private int currentAmmo;

    [SerializeField] private float reloadTime;
    [SerializeField] private TMP_Text reloadText;

    public IFireStrategy currentFireStrategy;
    public IReloadStrategy currentReloadStrategy;
    
    public IFireStrategy projectileFireStrategy;
    public IFireStrategy HitscanFireStrategy;
    
    public IReloadStrategy noReloadStrategy;
    public IReloadStrategy revolverReloadStrategy;
    
    private float nextFireTime;
    private bool isReloading;
    
    private void Awake()
    {
        HitscanFireStrategy = new HitscanFireStrategy(enemyLayerMask, Camera.main, gunDamage, gunAnimator);
        projectileFireStrategy =
            new ProjectileFireStategy(enemyLayerMask, Camera.main, gunDamage, gunAnimator, bulletPrefab);
        
        noReloadStrategy = new NoReloadStrategy();
        revolverReloadStrategy = new RevolverReloadStrategy(reloadTime);

        currentFireStrategy = HitscanFireStrategy;
        currentReloadStrategy = noReloadStrategy;
        UpdateAmmoUI();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && currentAmmo > 0 && !isReloading)
        {
            DeductAmmo();
            Shoot();
            nextFireTime = Time.time + timeBetweenShots;
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        currentFireStrategy.Fire();
    }

    private void DeductAmmo()
    {
        currentAmmo = currentReloadStrategy.DeductAmmo(currentAmmo);
        UpdateAmmoUI();
    }
    
    private void Reload()
    {
        isReloading = true;
        currentAmmo = currentReloadStrategy.Reload(currentAmmo, maxAmmo);
        UpdateAmmoUI();
        isReloading = false;
    }

    private void UpdateAmmoUI()
    {
        reloadText.text = currentAmmo + "/" + maxAmmo;
    }
}
