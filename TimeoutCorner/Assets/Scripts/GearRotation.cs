using UnityEngine;
using System.Collections;

public class GearRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 25f; // Speed of gear rotation

    [SerializeField] private float minRotationAngle = -360f; // Minimum allowed rotation in degrees
    [SerializeField] private float maxRotationAngle = 360f;  // Maximum allowed rotation in degrees

    [Header("Audio Settings")]
    public AudioClip dragSound; // Assignable sound effect for dragging
    private AudioSource audioSource;
    
    [Range(0f, 1f)] 
    public float volume = 1f; // Volume control (adjustable in Inspector)

    public float fadeOutDuration = 1f; // How long the sound lingers before fading out
    public float fadeOutDelay = 0.25f; // Time before fade-out starts after stopping dragging

    private bool isDragging = false;
    private float previousMouseY;
    private float totalRotation = 0f; // Each gear tracks its own rotation
    private Camera playerCamera;
    private Transform selectedGear; // Store the currently selected gear
    private Coroutine fadeOutCoroutine; // Store the fade-out coroutine to cancel it if needed

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogError("Main Camera not found! Ensure your camera is tagged as 'MainCamera'.");
        }

        // Ensure an AudioSource component is present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // Enable looping
        audioSource.playOnAwake = false; // Don't play automatically
        audioSource.volume = volume; // Set initial volume
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

                    // Cancel fade-out if it's running (sound should keep playing)
                    if (fadeOutCoroutine != null)
                    {
                        StopCoroutine(fadeOutCoroutine);
                        fadeOutCoroutine = null;
                        audioSource.volume = volume; // Reset volume immediately
                    }

                    // Play dragging sound with updated volume
                    if (dragSound != null && !audioSource.isPlaying)
                    {
                        audioSource.clip = dragSound;
                        audioSource.volume = volume; // Set volume before playing
                        audioSource.Play();
                    }
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

            // Start fade-out after a delay
            if (audioSource.isPlaying)
            {
                fadeOutCoroutine = StartCoroutine(DelayedFadeOut(fadeOutDelay, fadeOutDuration));
            }
        }
    }

    // Wait for delay before starting fade-out
    // Wait for delay before starting fade-out (runs regardless of timeScale)
    private IEnumerator DelayedFadeOut(float delay, float fadeDuration)
    {
        yield return new WaitForSecondsRealtime(delay); // Wait for delay before fading out

        float startVolume = audioSource.volume;
        float elapsed = 0f; // Track elapsed time

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time to remain independent of pausing
            float t = Mathf.Clamp01(elapsed / fadeDuration); // Ensure t stays between 0 and 1
            
            // Apply an exponential fade curve for a smoother effect
            audioSource.volume = Mathf.Lerp(startVolume, 0, Mathf.SmoothStep(0, 1, t)); 
            
            yield return null;

            // If dragging starts again, cancel the fade-out
            if (isDragging)
            {
                audioSource.volume = volume; // Reset volume immediately
                yield break;
            }
        }

        audioSource.Stop();
        audioSource.volume = volume; // Reset volume for next use
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
