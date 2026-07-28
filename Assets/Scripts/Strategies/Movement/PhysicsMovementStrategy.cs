using UnityEngine;

/// <summary>
/// ice based movement
/// </summary>
public class PhysicsMovementStrategy : IMovementRule
{
    private float acceleration;
    private float maxSpeed;
    private float friction;

    private Vector3 currentVelocity;

    private GameObject playerGameObject;

    public PhysicsMovementStrategy(GameObject playerGameObject, float acceleration, float maxSpeed, float friction)
    {
        this.playerGameObject = playerGameObject;
        this.acceleration = acceleration;
        this.maxSpeed = maxSpeed;
        this.friction = friction;
    }

    public Vector3 Move(Vector2 direction)
    {
        Vector3 inputDirection =
            playerGameObject.transform.right * direction.x +
            playerGameObject.transform.forward * direction.y;


        if (inputDirection != Vector3.zero)
        {
            currentVelocity += inputDirection * (acceleration * Time.deltaTime);

            currentVelocity = Vector3.ClampMagnitude(
                currentVelocity,
                maxSpeed
            );
        }
        else
        {
            currentVelocity = Vector3.Lerp(
                currentVelocity,
                Vector3.zero,
                friction * Time.deltaTime
            );
        }

        return currentVelocity;
    }

    public float Jump()
    {
        return 15f;
    }

    public Vector3 Dash()
    {
        return Vector3.zero;
    }
}