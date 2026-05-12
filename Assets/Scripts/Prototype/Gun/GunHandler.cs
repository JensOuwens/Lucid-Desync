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

    public IFireStrategy fireStrategy;
    public IReloadStrategy reloadStrategy;
    
    private float nextFireTime;
    private bool isReloading;
    
    private void Awake()
    {
        fireStrategy = new HitscanFireStrategy(enemyLayerMask, Camera.main, gunDamage, gunAnimator);
        reloadStrategy = new NoReloadStrategy();
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
        fireStrategy.Fire();
    }

    private void DeductAmmo()
    {
        currentAmmo = reloadStrategy.DeductAmmo(currentAmmo);
    }
    
    private void Reload()
    {
        isReloading = true;
        currentAmmo = reloadStrategy.Reload(currentAmmo, maxAmmo);
        isReloading = false;
    }

    private void UpdateAmmoUI()
    {
        reloadText.text = currentAmmo + "/" + maxAmmo;
    }
}
