using UnityEngine;

public class DashCommand : ICommand
{
    private RuleManager ruleManager;

    public DashCommand(RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }
    
    public void Execute()
    {
        ruleManager.CurrentMovementRule.Dash();
    }
}
