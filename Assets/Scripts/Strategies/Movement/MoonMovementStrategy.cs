using UnityEngine;

/// <summary>
/// Halo type Movement
/// </summary>
public class MoonMovementStrategy : IMovementRule
{
    private float moveSpeed;
    private CharacterController playerCharacterController;
    private GameObject playerGameObject;

    private float jumpForce;
    private float verticalVelocity;

    public MoonMovementStrategy (float moveSpeed, CharacterController playerCharacterController, GameObject playerGameObject, float jumpForce)
    {
        this.moveSpeed = moveSpeed;
        this.playerCharacterController = playerCharacterController;
        this.playerGameObject = playerGameObject;
        this.jumpForce = jumpForce;
        
    }
    
    public void Move(Vector2 direction)
    {
        direction.Normalize();

        Vector3 movement = playerGameObject.transform.right * direction.x + playerGameObject.transform.forward * direction.y;
        
        playerCharacterController.Move( movement * (moveSpeed * Time.deltaTime));
    }

    public float Jump()
    {
        if (playerCharacterController.isGrounded)
        {
            verticalVelocity = jumpForce;
        }

        return verticalVelocity;
    }

    public void Dash()
    {
        //do nothing
    }
}
