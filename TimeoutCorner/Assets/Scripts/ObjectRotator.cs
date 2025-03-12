using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ObjectRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    public GearRotation gear; // The gear that controls this object's rotation
    public float rotationMultiplier = 1f; // Multiplier to control relative rotation

    private Rigidbody rb;
    private float previousGearRotation; // Store previous gear rotation

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (gear == null)
        {
            Debug.LogError($"No Gear assigned to {gameObject.name}'s ObjectRotator!");
        }

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        previousGearRotation = gear.GetTotalRotation(); // Initialize previous rotation
    }

    void Update()
    {
        if (gear != null)
        {
            // Calculate rotation difference (how much the gear has rotated since last frame)
            float rotationDelta = (gear.GetTotalRotation() - previousGearRotation) * rotationMultiplier;

            // Apply relative rotation
            transform.Rotate(Vector3.up * rotationDelta);

            // Update previous rotation for next frame
            previousGearRotation = gear.GetTotalRotation();
        }
    }
}
