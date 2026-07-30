using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class RevolverReload : IReloadRule
{
    private int currentAmmo;
    private int maxAmmo;
    private float reloadTimeInSeconds;

    public RevolverReload(int maxAmmo, float reloadTime)
    {
        this.maxAmmo = maxAmmo;
        this.reloadTimeInSeconds = reloadTime;

        currentAmmo = this.maxAmmo;
    }
    
    public async Task Reload()
    {
        if (currentAmmo < maxAmmo)
        {
            float reloadTimeInMS = reloadTimeInSeconds * 1000;
            await Task.Delay((int)reloadTimeInMS);
            currentAmmo = maxAmmo;
        }
    }

    public int GetAmmo()
    {
        return currentAmmo;
    }

    public void DeductAmmo()
    {
        currentAmmo--;
    }
}
