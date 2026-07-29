using System.Threading.Tasks;
using UnityEngine;

public class NoReloadRule : IReloadRule
{
    public Task Reload()
    {
        //do nothing
        return Task.CompletedTask;
    }

    public int GetAmmo()
    {
        return 1;
    }

    public void DeductAmmo()
    {
        //do nothing
    }
}
