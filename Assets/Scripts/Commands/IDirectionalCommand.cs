using UnityEngine;

public interface IDirectionalCommand
{
    Vector3 Execute(Vector2 direction);
}
