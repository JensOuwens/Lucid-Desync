using UnityEngine;

public interface IGravityRule
{
        Vector3 ApplyGravity(Vector3 velocity, float deltaTime);
}
