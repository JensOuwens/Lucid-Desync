using System.Threading.Tasks;
using UnityEngine;

public interface IReloadRule
{
    public Task Reload();

    public int GetAmmo();
    
    public void DeductAmmo();
}
