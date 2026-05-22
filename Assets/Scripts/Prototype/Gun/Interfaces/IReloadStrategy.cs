using UnityEngine;

public interface IReloadStrategy
{
    public int Reload(int currentAmmo, int maxAmmo);
    
    public int DeductAmmo(int currentAmmo);

    public bool CanReload();
}
