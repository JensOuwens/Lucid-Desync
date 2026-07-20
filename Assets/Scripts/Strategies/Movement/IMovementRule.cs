using UnityEngine;

public interface IMovementRule
{
    public void Move(Vector2 direction);

    public void Jump();

    public void Dash();
}
