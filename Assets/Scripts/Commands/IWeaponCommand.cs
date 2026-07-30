using UnityEngine;

public interface IWeaponCommand
{
    bool ExecuteTap();
    void ExecuteHold();
    bool ExecuteRelease();
}
