using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float sensitivity = 2f;
    
    [Header("References")]
    public Transform playerBody; // Assign the Player object (Parent of the Head)

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // Unlock the cursor
    }

    void Update()
    {
        Cursor.visible = false; // Lock cursor to center
        HandleMouseLook();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Rotate the head up/down (clamping to prevent flipping)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player's body left/right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
