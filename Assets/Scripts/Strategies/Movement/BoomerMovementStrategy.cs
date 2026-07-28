using UnityEngine;

/// <summary>
/// boomer shooter type movement
/// </summary>
public class BoomerMovementStrategy : IMovementRule
{
    private float moveSpeed;
    private GameObject playerGameObject;

    public BoomerMovementStrategy (float moveSpeed, GameObject playerGameObject)
    {
        this.moveSpeed = moveSpeed;
        this.playerGameObject = playerGameObject;
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
        return 0;
    }

    public Vector3 Dash()
    {
        return Vector3.zero;
    }
}
