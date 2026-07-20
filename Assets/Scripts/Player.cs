using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles action mapping, sends inputs through to rule manager.
/// </summary>
public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private RuleManager ruleManager;

    private PlayerInput inputActions;

    private void Awake()
    {
        inputActions = new PlayerInput();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void Update()
    {
        // Movement
        Vector2 directionalInput = inputActions.Player.Move.ReadValue<Vector2>();
        //ruleManager.Move(directionalInput);

        // Jump
        if (inputActions.Player.Jump.WasPressedThisFrame())
        {
            //ruleManager.Jump();
        }

        // Dash
        if (inputActions.Player.Sprint.WasPressedThisFrame())
        {
            //ruleManager.Dash();
        }

        // Reload
        if (inputActions.Player.Reload.WasPressedThisFrame())
        {
            //ruleManager.Reload();
        }

        // Fire pressed
        if (inputActions.Player.Attack.WasPressedThisFrame())
        {
            //ruleManager.FirePressed();
        }

        // Fire held
        if (inputActions.Player.Attack.IsPressed())
        {
            //ruleManager.FireHeld();
        }

        // Fire released
        if (inputActions.Player.Attack.WasReleasedThisFrame())
        {
            //ruleManager.FireReleased();
        }
    }

    public void Hurt(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
    
    private void OnDisable()
    {
        inputActions.Disable();
    }
}