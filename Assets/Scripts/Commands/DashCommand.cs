using UnityEngine;

public class DashCommand : ICommand
{
    private RuleManager ruleManager;

    private DashCommand(RuleManager ruleManager)
    {
        this.ruleManager = ruleManager;
    }
    
    public void Execute()
    {
        //call the rulemanager command
    }
}
