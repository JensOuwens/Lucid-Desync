using UnityEngine;

public class DashCommand : IDashCommand
{
    private RuleManager ruleManager;

    public DashCommand(RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }
    
    public Vector3 Execute()
    {
        return ruleManager.CurrentMovementRule.Dash();
    }
}
