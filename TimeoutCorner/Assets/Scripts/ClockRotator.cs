using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ClockRotator : MonoBehaviour
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
            Debug.LogError($"No Gear assigned to {gameObject.name}'s ClockRotator!");
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
            float currentGearRotation = gear.GetTotalRotation();
            float rotationDelta = (currentGearRotation - previousGearRotation) * rotationMultiplier;

            // Rotate based on the gear's movement (can go both ways)
            transform.Rotate(Vector3.up * rotationDelta); // Using Z-axis for a clock-like rotation

            previousGearRotation = currentGearRotation; // Update previous rotation for next frame
        }
    }
}
