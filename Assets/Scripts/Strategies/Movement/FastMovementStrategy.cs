using UnityEngine;

/// <summary>
/// faster movement with dash
/// </summary>
public class FastMovementStrategy : IMovementRule
{
    private float moveSpeed;
    private GameObject playerGameObject;
    
    private float jumpForce;
    private float dashForce;

    public FastMovementStrategy (float moveSpeed, GameObject playerGameObject, float jumpForce, float  dashForce)
    {
        this.moveSpeed = moveSpeed;
        this.playerGameObject = playerGameObject;
        this.jumpForce = jumpForce;
        this.dashForce = dashForce;
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
        return playerGameObject.transform.forward * dashForce;
    }
}
