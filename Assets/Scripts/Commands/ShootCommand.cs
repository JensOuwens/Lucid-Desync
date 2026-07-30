using UnityEngine;

public class ShootCommand : IWeaponCommand
{
    private RuleManager ruleManager;
    public ShootCommand(RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }

    public bool ExecuteTap()
    {
        return ruleManager.CurrentWeaponRule.Press();
    }

    public void ExecuteHold()
    {
        ruleManager.CurrentWeaponRule.Hold();
    }

    public bool ExecuteRelease()
    {
        return ruleManager.CurrentWeaponRule.Release();
    }
}
