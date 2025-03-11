using UnityEngine;

public class GearRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 25f; // Speed of gear rotation

    [SerializeField] private float minRotationAngle = -360f; // Minimum allowed rotation in degrees
    [SerializeField] private float maxRotationAngle = 360f;  // Maximum allowed rotation in degrees

    private bool isDragging = false;
    private float previousMouseY;
    private float totalRotation = 0f; // Each gear tracks its own rotation

    private Camera playerCamera;
    private Transform selectedGear; // Store the currently selected gear

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("Main Camera not found! Ensure your camera is tagged as 'MainCamera'.");
        }
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to start dragging
        {
            Ray ray = new Ray(playerCamera.transform.position + playerCamera.transform.forward * -0.2f, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide))
            {
                Debug.Log("Ray hit: " + hit.collider.name);

                if (hit.collider.CompareTag("Gear")) // Only select the clicked gear
                {
                    Debug.Log("GEAR HIT!");
                    isDragging = true;
                    selectedGear = hit.transform; // Store the specific gear that was clicked
                    previousMouseY = Input.mousePosition.y;
                }
            }
        }

        if (isDragging && selectedGear == transform) // ✅ Only rotate the specific clicked gear
        {
            float deltaY = Input.mousePosition.y - previousMouseY;
            float rotationAmount = deltaY * -rotationSpeed * Time.deltaTime;

            // Get the gear's current rotation
            float currentRotation = selectedGear.localEulerAngles.x;
            currentRotation = (currentRotation > 180) ? currentRotation - 360 : currentRotation; // Convert to -180 to 180 range

            // Check if the new rotation stays within limits
            float newRotation = currentRotation + rotationAmount;

            if (newRotation >= minRotationAngle && newRotation <= maxRotationAngle)
            {
                selectedGear.Rotate(Vector3.right, rotationAmount);
                SetTotalRotation(rotationAmount); // ✅ Only update this gear's total rotation
            }

            previousMouseY = Input.mousePosition.y;
        }

        if (Input.GetMouseButtonUp(0)) // Stop dragging
        {
            isDragging = false;
            selectedGear = null; // Deselect gear
        }
    }

    // Private setter to ensure total rotation is only modified inside this script
    private void SetTotalRotation(float deltaRotation)
    {
        totalRotation += deltaRotation;
    }

    // Public method for ObjectLifter to access
    public float GetTotalRotation()
    {
        return totalRotation;
    }
}