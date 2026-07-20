using UnityEngine;

public class MoveCommand : IDirectionalCommand
{
    private RuleManager ruleManager;

    private MoveCommand (RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }

    public void Execute(Vector2 direction)
    {
        //call the rulemanager command
    }
}
