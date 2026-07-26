using UnityEngine;

public class JumpCommand : IFloatCommand
{
    private RuleManager ruleManager;

    public JumpCommand(RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }
    
    public float Execute()
    {
        return ruleManager.CurrentMovementRule.Jump();
    }
}
