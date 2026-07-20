using UnityEngine;

public class RuleManager : MonoBehaviour
{
    //current commands
    private ICommand currentShootCommand;
    private IDirectionalCommand currentMoveCommand;
    private ICommand currentJumpCommand;
    private ICommand currentDashCommand;
    private ICommand currentReloadCommand;
    
    //shoot commands
    private IWeaponCommand hitscanShootCommand;
    private IWeaponCommand chargeShootCommand;
    private IWeaponCommand projectileShootCommand;
    private IWeaponCommand swordShootCommand;
    
    //move commands
    private IDirectionalCommand boomerMoveCommand;
    private IDirectionalCommand moonMoveCommand;
    private IDirectionalCommand physicsMoveCommand;
    private IDirectionalCommand fastMoveCommand;

    //jump commands
    private ICommand noJumpCommand;
    private ICommand normalJumpCommand;
    private ICommand moonJumpCommand;
    private ICommand wallJumpCommand;
    
    //dash commands
    private ICommand noDashCommand;
    private ICommand normalDashCommand;
    
    //reload commands
    private ICommand noReloadCommand;
    private ICommand RevolverReloadCommand;
    private ICommand swordReloadCommand;
    
    


}
