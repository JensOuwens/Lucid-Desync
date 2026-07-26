using UnityEngine;

/// <summary>
/// boomer shooter type movement
/// </summary>
public class BoomerMovementStrategy : IMovementRule
{
    private float moveSpeed;
    private CharacterController playerCharacterController;

    public BoomerMovementStrategy (float moveSpeed, CharacterController playerCharacterController)
    {
        this.moveSpeed = moveSpeed;
        this.playerCharacterController = playerCharacterController;
    }
    
    public void Move(Vector2 direction)
    {
        direction.Normalize();
        playerCharacterController.Move( new Vector3(direction.x, 0, direction.y) * (moveSpeed * Time.deltaTime));
    }

    public void Jump()
    {
        //do nothing
    }

    public void Dash()
    {
        //do nothing
    }
}
