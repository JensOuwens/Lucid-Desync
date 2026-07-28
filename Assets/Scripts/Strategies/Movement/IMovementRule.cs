using UnityEngine;

public interface IMovementRule
{
    public Vector3 Move(Vector2 direction);

    public float Jump();

    public Vector3 Dash();
}
