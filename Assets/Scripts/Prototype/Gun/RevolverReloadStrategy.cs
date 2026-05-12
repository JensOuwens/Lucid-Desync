using UnityEngine;

/// <summary>
/// TODO: reload functionality
/// </summary>
public class RevolverReloadStrategy : IReloadStrategy
{
   private float reloadTime;
   
    public RevolverReloadStrategy(float reloadTime)
    {
        this.reloadTime = reloadTime;
    }
    
    public int Reload(int currentAmmo, int maxAmmo)
    {
        return 0;
    }

    public int DeductAmmo(int currentAmmo)
    {
        return currentAmmo--;
    }
}
