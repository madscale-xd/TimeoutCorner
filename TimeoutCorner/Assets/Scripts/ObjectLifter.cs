using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectLifter : MonoBehaviour
{
    [Header("Lift Settings")]
    public GearRotation gear; // Each wall has its own assigned gear
    public float liftSpeed = 0.1f; // Speed at which the object moves upward
    public float maxHeight = 5f; // Maximum height the object can reach
    public float minHeight = 0f; // Minimum height the object can go down to
    public float smoothFactor = 5f; // Controls smoothness of movement

    private Rigidbody rb;
    private float initialY; // Store the starting Y position

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (gear == null)
        {
            Debug.LogError($"No Gear assigned to {gameObject.name}'s ObjectLifter!");
        }

        if (rb != null)
        {
            rb.useGravity = false; // Disable gravity to avoid falling
            rb.isKinematic = true; // Allow controlled movement
        }

        initialY = transform.position.y; // Store initial position
    }

    void FixedUpdate()
    {
        if (gear != null)
        {
            float totalRotation = gear.GetTotalRotation(); // ✅ Each wall gets its own gear's rotation

            // Calculate new height based on gear rotation
            float targetY = initialY + (totalRotation * liftSpeed);

            // Clamp the height between minHeight and maxHeight
            targetY = Mathf.Clamp(targetY, initialY - minHeight, initialY + maxHeight);

            // Smoothly interpolate between current position and target position
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            rb.position = Vector3.Lerp(rb.position, targetPosition, Time.fixedDeltaTime * smoothFactor);
        }
    }
}
