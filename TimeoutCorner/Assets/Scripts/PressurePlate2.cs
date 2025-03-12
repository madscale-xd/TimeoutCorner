using UnityEngine;

public class PressurePlate2 : MonoBehaviour
{
    [Header("Target Object to Rotate")]
    public Transform targetObject; // The object that will rotate
    public Vector3 rotationAxis = Vector3.right; // Axis to rotate around
    public float rotationAngle = 90f; // Total degrees to rotate
    public bool resetOnExit = true; // If true, returns to original rotation

    private Quaternion initialRotation; // Stores the original rotation
    private bool isActivated = false; // Track if the plate is pressed

    void Start()
    {
        if (targetObject != null)
        {
            initialRotation = targetObject.rotation; // Store original rotation
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated && other.gameObject.name == "Switch" && targetObject != null)
        {
            isActivated = true;
            targetObject.rotation = Quaternion.Euler(rotationAxis * rotationAngle) * targetObject.rotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isActivated && resetOnExit && other.gameObject.name == "Switch" && targetObject != null)
        {
            isActivated = false;
            targetObject.rotation = initialRotation; // Reset to original rotation
        }
    }
}
