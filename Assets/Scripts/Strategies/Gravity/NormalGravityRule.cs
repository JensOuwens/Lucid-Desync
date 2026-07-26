using UnityEngine;

public class NormalGravityRule : IGravityRule
{
    public Vector3 ApplyGravity(Vector3 velocity, float deltaTime)
    {
        velocity.y -= 19.8f * deltaTime;
        return velocity;
    }
}
