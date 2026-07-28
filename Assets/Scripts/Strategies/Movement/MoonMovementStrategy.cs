using UnityEngine;

/// <summary>
/// Halo type Movement
/// </summary>
public class MoonMovementStrategy : IMovementRule
{
    private float moveSpeed;
    private GameObject playerGameObject;

    private float jumpForce;

    public MoonMovementStrategy (float moveSpeed, GameObject playerGameObject, float jumpForce)
    {
        this.moveSpeed = moveSpeed;
        this.playerGameObject = playerGameObject;
        this.jumpForce = jumpForce;
        
    }
    
    public Vector3 Move(Vector2 direction)
    {
        direction.Normalize();

        Vector3 movement =
            playerGameObject.transform.right * direction.x +
            playerGameObject.transform.forward * direction.y;

        return movement * moveSpeed;
    }

    public float Jump()
    {
        return jumpForce;
    }

    public Vector3 Dash()
    {
        return Vector3.zero;
    }
}