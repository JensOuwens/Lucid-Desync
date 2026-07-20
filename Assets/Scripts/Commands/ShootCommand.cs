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
        //call the rulemanager command
    }

    public void ExecuteHold()
    {
        //call the rulemanager command
    }

    public void ExecuteRelease()
    {
        //call the rulemanager command
    }
}
