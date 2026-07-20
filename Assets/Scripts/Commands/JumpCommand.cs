using UnityEngine;

public class JumpCommand : ICommand
{
    private RuleManager ruleManager;

    public JumpCommand(RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }
    
    public void Execute()
    {
        ruleManager.CurrentMovementRule.Jump();
    }
}
