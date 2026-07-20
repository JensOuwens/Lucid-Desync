using UnityEngine;

public class ShootCommand : IWeaponCommand
{
    private RuleManager ruleManager;
    public ShootCommand(RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }

    public void ExecuteTap()
    {
        ruleManager.CurrentWeaponRule.Press();
    }

    public void ExecuteHold()
    {
        ruleManager.CurrentWeaponRule.Hold();
    }

    public void ExecuteRelease()
    {
        ruleManager.CurrentWeaponRule.Release();
    }
}
