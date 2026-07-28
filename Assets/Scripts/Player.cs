using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles action mapping, sends inputs through to rule manager.
/// </summary>
public class Player : MonoBehaviour, IDamagable
{
    [Header("References")]
    [SerializeField] private RuleManager ruleManager;
    [SerializeField] private FirstPersonCamera firstPersonCamera;
    
    [Header("ground check")]
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private PlayerInput inputActions;
    
    //commands
    private IWeaponCommand shootCommand;
    private IDirectionalCommand moveCommand;
    private IFloatCommand jumpCommand;
    private ICommand dashCommand;
    private ICommand reloadCommand;
    
    private CharacterController playerCharacterController;
    
    private Vector3 velocity;

    private void Awake()
    {
        inputActions = new PlayerInput();

        shootCommand = new ShootCommand(ruleManager);
        moveCommand = new MoveCommand(ruleManager);
        jumpCommand = new JumpCommand(ruleManager);
        dashCommand = new DashCommand(ruleManager);
        reloadCommand = new ReloadCommand(ruleManager);

        playerCharacterController = this.gameObject.GetComponent<CharacterController>();
            
        ruleManager.SetupStrategies(playerCharacterController, this.gameObject);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Update()
    {
        Debug.Log(IsGrounded());
        
        // Movement
        Vector2 directionalInput = inputActions.Player.Move.ReadValue<Vector2>();
        moveCommand.Execute(directionalInput);
        
        //Camera
        Vector2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();
        firstPersonCamera.Look(lookInput);

        // Jump
        if (inputActions.Player.Jump.WasPressedThisFrame())
        {
            if (!IsGrounded()) return;
            
            float jumpVelocity  = jumpCommand.Execute();
            
            if (jumpVelocity > 0f)
            {
                velocity.y = jumpVelocity;
            }
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
        
        HandleGravity();
    }
    
    private void HandleGravity()
    {
        //gravity
        if (playerCharacterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        
        velocity = ruleManager.CurrentGravityRule.ApplyGravity(velocity, Time.deltaTime);
        
        playerCharacterController.Move(velocity * Time.deltaTime);
    }
    
    private bool IsGrounded()
    {
        Vector3 origin = transform.position;

        return Physics.Raycast(
            origin,
            Vector3.down,
            groundCheckDistance,
            groundMask
        );
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