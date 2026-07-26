using UnityEngine;

/// <summary>
/// boomer shooter type movement
/// </summary>
public class BoomerMovementStrategy : IMovementRule
{
    private float moveSpeed;
    private CharacterController playerCharacterController;
    private GameObject playerGameObject;

    public BoomerMovementStrategy (float moveSpeed, CharacterController playerCharacterController, GameObject playerGameObject)
    {
        this.moveSpeed = moveSpeed;
        this.playerCharacterController = playerCharacterController;
        this.playerGameObject = playerGameObject;
    }
    
    public void Move(Vector2 direction)
    {
        direction.Normalize();

        Vector3 movement = playerGameObject.transform.right * direction.x + playerGameObject.transform.forward * direction.y;
        
        playerCharacterController.Move( movement * (moveSpeed * Time.deltaTime));
    }

    public float Jump()
    {
        return 0;
    }

    public void Dash()
    {
        //do nothing
    }
}
