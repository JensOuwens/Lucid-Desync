using UnityEngine;

public class ReloadCommand : ICommand
{
    private RuleManager ruleManager;

    public ReloadCommand(RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }
    
    public void Execute()
    {
        ruleManager.CurrentReloadRule.Reload();
    }
}
