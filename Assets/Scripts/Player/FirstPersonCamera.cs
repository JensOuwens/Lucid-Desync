using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float sensitivity = 2f;

    private float pitch = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Look(Vector2 lookInput)
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        // Rotate player horizontally
        player.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -89f, 89f);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
