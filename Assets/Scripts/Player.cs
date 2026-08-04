using System.Collections;
using System.Collections.Generic;
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
    private IDashCommand dashCommand;
    private ICommand reloadCommand;
    
    private CharacterController playerCharacterController;
    
    //velocity
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    
    //dash stuff
    private Vector3 dashVelocity;
    private float dashTimer;
    private float dashCooldownTimer;
    
    //health
    private Coroutine shieldRechargeCoroutine;

    [Header("Dash stuff")]
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

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
        // Movement
        Vector2 directionalInput = inputActions.Player.Move.ReadValue<Vector2>();

        horizontalVelocity =
            ruleManager.CurrentMovementRule.Move(directionalInput);
        
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
                verticalVelocity  = jumpVelocity;
            }
        }

        // Dash
        if (inputActions.Player.Sprint.WasPressedThisFrame())
        {
            if (dashCooldownTimer <= 0)
            {
                dashVelocity = dashCommand.Execute();
                dashTimer = dashDuration;
                dashCooldownTimer = dashCooldown;
            }
        }

        // Reload
        if (inputActions.Player.Reload.WasPressedThisFrame())
        {
            reloadCommand.Execute();
            Hurt(-30);
        }

        // Fire pressed
        if (inputActions.Player.Attack.WasPressedThisFrame())
        {
            if (ruleManager.CurrentReloadRule.GetAmmo() > 0)
            {
                bool fired = shootCommand.ExecuteTap();

                if (fired)
                {
                    ruleManager.CurrentReloadRule.DeductAmmo();
                }
            }
            
            Hurt(15);
        }

        // Fire held
        if (inputActions.Player.Attack.IsPressed())
        {
            shootCommand.ExecuteHold();
        }

        // Fire released
        if (inputActions.Player.Attack.WasReleasedThisFrame())
        {
            bool fired = shootCommand.ExecuteRelease();

            if (fired)
            {
                ruleManager.CurrentReloadRule.DeductAmmo();
            }
        }
        
        HandleGravity();
    }
    
    //handles application of velocity
    private void HandleGravity()
    {
        HandleDash();
        
        if (IsGrounded() && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
        }

        Vector3 gravityVelocity =
            ruleManager.CurrentGravityRule.ApplyGravity(
                Vector3.up * verticalVelocity,
                Time.deltaTime
            );

        verticalVelocity = gravityVelocity.y;


        Vector3 finalVelocity =
            horizontalVelocity +
            dashVelocity +
            Vector3.up * verticalVelocity;


        playerCharacterController.Move(
            finalVelocity * Time.deltaTime
        );
    }
    
    private void HandleDash()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        else
        {
            dashVelocity = Vector3.zero;
        }
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

    public void Hurt(float damage)
    {
        ruleManager.CurrentHealthRule.Damage(damage);

        if (ruleManager.CurrentHealthRule is ShieldHealthRule shield)
        {
            if (shieldRechargeCoroutine != null)
            {
                StopCoroutine(shieldRechargeCoroutine);
            }

            shieldRechargeCoroutine = StartCoroutine(RechargeShield(shield));
        }
    }
    
    private IEnumerator RechargeShield(ShieldHealthRule shield)
    {
        yield return new WaitForSeconds(shield.RechargeCooldown);

        while (shield.ReturnHealth() < shield.MaxHealth)
        {
            shield.Heal(shield.RechargeRate * Time.deltaTime);

            yield return null;
        }

        shieldRechargeCoroutine = null;
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