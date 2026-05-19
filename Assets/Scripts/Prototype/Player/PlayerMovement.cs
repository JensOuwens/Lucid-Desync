using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// TODO: death logic
/// </summary>
public class PlayerMovement : MonoBehaviour, IDamagable
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private int maxHealth;
    [SerializeField] TMP_Text healthtext;

    private CharacterController controller;
    private PlayerInput inputActions;

    private Vector2 moveInput;
    private float verticalVelocity;

    private int currentHealth;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInput();
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Disable();
    }

    private void Update()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f; // keeps grounded stable
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * moveInput.y + right * moveInput.x;

        controller.Move(move * ((moveSpeed * 10) * Time.deltaTime));

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * (verticalVelocity * Time.deltaTime));
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Hurt(int damage)
    {
        currentHealth -= damage;
        UpdateHealthText();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    
    private void UpdateHealthText()
    {
        healthtext.text = currentHealth + "/" + maxHealth;
    }
}