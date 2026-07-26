using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles action mapping, sends inputs through to rule manager.
/// </summary>
public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private RuleManager ruleManager;
    [SerializeField] private FirstPersonCamera firstPersonCamera;

    private PlayerInput inputActions;
    
    //commands
    private IWeaponCommand shootCommand;
    private IDirectionalCommand moveCommand;
    private ICommand jumpCommand;
    private ICommand dashCommand;
    private ICommand reloadCommand;

    private void Awake()
    {
        inputActions = new PlayerInput();

        shootCommand = new ShootCommand(ruleManager);
        moveCommand = new MoveCommand(ruleManager);
        jumpCommand = new JumpCommand(ruleManager);
        dashCommand = new DashCommand(ruleManager);
        reloadCommand = new ReloadCommand(ruleManager);
        
        ruleManager.SetupStrategies(this.gameObject.GetComponent<CharacterController>(), this.gameObject);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Update()
    {
        // Movement
        Vector2 directionalInput = inputActions.Player.Move.ReadValue<Vector2>();
        moveCommand.Execute(directionalInput);
        
        //Camera
        Vector2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();
        firstPersonCamera.Look(lookInput);

        // Jump
        if (inputActions.Player.Jump.WasPressedThisFrame())
        {
            jumpCommand.Execute();
        }

        // Dash
        if (inputActions.Player.Sprint.WasPressedThisFrame())
        {
            dashCommand.Execute();
        }

        // Reload
        if (inputActions.Player.Reload.WasPressedThisFrame())
        {
            reloadCommand.Execute();
        }

        // Fire pressed
        if (inputActions.Player.Attack.WasPressedThisFrame())
        {
            shootCommand.ExecuteTap();
        }

        // Fire held
        if (inputActions.Player.Attack.IsPressed())
        {
            shootCommand.ExecuteHold();
        }

        // Fire released
        if (inputActions.Player.Attack.WasReleasedThisFrame())
        {
            shootCommand.ExecuteRelease();
        }
    }

    public void Hurt(int damage)
    {
        ruleManager.CurrentHealthRule.Damage();
    }

    public void Die()
    {
        ruleManager.CurrentHealthRule.Die();
    }
    
    private void OnDisable()
    {
        inputActions.Disable();
    }
}