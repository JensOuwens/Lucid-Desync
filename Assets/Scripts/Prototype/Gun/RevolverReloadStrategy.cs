using UnityEngine;

public class RevolverReloadStrategy : IReloadStrategy
{
    public RevolverReloadStrategy(float reloadTime)
    {
    }

    public int Reload(int currentAmmo, int maxAmmo)
    {
        return maxAmmo;
    }

    public int DeductAmmo(int currentAmmo)
    {
        return currentAmmo - 1;
    }

    public bool CanReload()
    {
        return true;
    }
}