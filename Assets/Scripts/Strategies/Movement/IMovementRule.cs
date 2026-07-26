using UnityEngine;

public interface IMovementRule
{
    public void Move(Vector2 direction);

    public float Jump();

    public void Dash();
}
