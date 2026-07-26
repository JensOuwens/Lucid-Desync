using UnityEngine;

public class NormalGravityRule : IGravityRule
{
    float gravityMagnitude = 19.8f;
    public Vector3 ApplyGravity(Vector3 velocity, float deltaTime)
    {
        velocity.y -= 19.8f * deltaTime;
        return velocity;
    }

    public float GetGravityMagnitude()
    {
        return gravityMagnitude;
    }
}
