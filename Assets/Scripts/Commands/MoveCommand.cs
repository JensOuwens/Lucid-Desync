using UnityEngine;

public class MoveCommand : IDirectionalCommand
{
    private RuleManager ruleManager;

    public MoveCommand (RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }

    public Vector3 Execute(Vector2 direction)
    {
        return ruleManager.CurrentMovementRule.Move(direction);
    }
}
