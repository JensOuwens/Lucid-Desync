using UnityEngine;

public class MoonGravityRule : IGravityRule
{
    public Vector3 ApplyGravity(Vector3 velocity, float deltaTime)
    {
        velocity.y -= 3.267f * deltaTime;
        return velocity;
    }
}
