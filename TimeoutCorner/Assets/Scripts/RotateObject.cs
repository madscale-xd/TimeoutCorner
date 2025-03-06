using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 50f, 0f); // Rotation speed per axis

    [Header("Bob Settings")]
    [SerializeField] private float bobSpeed = 2f;   // Speed of the bobbing motion
    [SerializeField] private float bobHeight = 0.5f; // How high it bobs

    private Vector3 startPos; // Stores the initial position

    void Start()
    {
        startPos = transform.position; // Save starting position
    }

    void Update()
    {
        // Rotate the object
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // Bob up and down using sine wave
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPos + new Vector3(0f, bobOffset, 0f);
    }
}
