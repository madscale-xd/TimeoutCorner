using UnityEngine;

public class BoatScript : MonoBehaviour
{
    public float platformSpeed = -1.1f; // Initial downward speed
    private Rigidbody rb;
    private bool movingSideways = false;
    private TimerScript timerScript;
    private float lastResetTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Prevents unwanted physics interactions

        // Find the GameObject named "TimerUI" and get TimerScript from it
        GameObject timerObject = GameObject.Find("TimerUI");
        if (timerObject != null)
        {
            timerScript = timerObject.GetComponent<TimerScript>();
            lastResetTime = Time.time; // Initialize reset time
        }
        else
        {
            Debug.LogError("⚠️ TimerUI GameObject not found in the scene!");
        }
    }

    void FixedUpdate()
    {
        if (timerScript == null) return; // Prevent errors if the timer script isn't found

        float elapsedTime = Time.time - lastResetTime; // Time since last reset

        if (elapsedTime < 13f)
        {
            // Move downward for the first 13 seconds after reset
            rb.MovePosition(rb.position + new Vector3(0, platformSpeed * Time.fixedDeltaTime, 0));
        }
        else
        {
            if (!movingSideways)
            {
                movingSideways = true; // Switch to sideways movement only once
                platformSpeed *= 2; // Double the speed
            }

            // Move left (negative X direction) after 13 seconds have passed since reset
            rb.MovePosition(rb.position + new Vector3(platformSpeed * Time.fixedDeltaTime, 0, 0));
        }
    }

    // Call this method when the timer resets
    public void OnTimerReset()
    {
        lastResetTime = Time.time;
        movingSideways = false; // Reset movement state
        platformSpeed = -1.1f; // Reset speed to downward speed
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Apply only the boat's horizontal movement to the player
                playerRb.velocity = new Vector3(platformSpeed, playerRb.velocity.y, playerRb.velocity.z);
            }
        }
    }
}
