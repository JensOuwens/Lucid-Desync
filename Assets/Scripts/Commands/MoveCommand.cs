using UnityEngine;

public class MoveCommand : IDirectionalCommand
{
    private RuleManager ruleManager;

    public MoveCommand (RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }

    public void Execute(Vector2 direction)
    {
        ruleManager.CurrentMovementRule.Move(direction);
    }
}
