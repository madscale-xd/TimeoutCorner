using UnityEngine;
using System.Collections;

public class ClockRotationTrigger : MonoBehaviour
{
    [Header("Trigger Objects")]
    public MinTrigger minTrigger; // Assignable MinTrigger component
    public HourTrigger hourTrigger; // Assignable HourTrigger component

    [Header("Rotation Settings")]
    public float rotationAngle = 30f; // Amount to rotate on the Y-axis
    public float rotationDuration = 3f; // Time in seconds to complete the rotation

    [Header("Audio Settings")]
    public AudioClip rotationSound; // Assignable audio clip
    private AudioSource audioSource;

    private bool hasRotated = false; // One-time flag

    private void Start()
    {
        // Ensure an AudioSource component is present
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        // Prevent multiple executions
        if (hasRotated) return;

        // Check if both objects are inside their respective triggers
        if (minTrigger != null && hourTrigger != null)
        {
            if (minTrigger.IsObjectInside() && hourTrigger.IsObjectInside())
            {
                StartCoroutine(RotateClockSmoothly());
            }
        }
    }

    private IEnumerator RotateClockSmoothly()
    {
        hasRotated = true; // Set the flag so it never executes again

        // Play audio if an audio clip is assigned
        if (rotationSound != null)
        {
            audioSource.PlayOneShot(rotationSound);
        }

        Quaternion startRotation = transform.rotation; // Store the current rotation
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, rotationAngle, 0); // Calculate target rotation

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final rotation is set correctly
        transform.rotation = targetRotation;

        Debug.Log("✅ Smooth Rotation applied!");
    }
}
