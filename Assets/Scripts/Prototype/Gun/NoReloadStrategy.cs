using UnityEngine;

public class NoReloadStrategy : IReloadStrategy
{
    public int Reload(int currentAmmo, int maxAmmo)
    {
        return maxAmmo;
    }

    public int DeductAmmo(int currentAmmo)
    {
        return currentAmmo;
    }
}
