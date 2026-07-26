using UnityEngine;

public class MoonGravityRule : IGravityRule
{
    float gravityMagnitude = 3.267f ;
    public Vector3 ApplyGravity(Vector3 velocity, float deltaTime)
    {
        velocity.y -= gravityMagnitude * deltaTime;
        return velocity;
    }

    public float GetGravityMagnitude()
    {
        return gravityMagnitude;
    }
}
