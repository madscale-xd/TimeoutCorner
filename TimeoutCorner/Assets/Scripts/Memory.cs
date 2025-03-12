using UnityEngine;
using TMPro;
using System.Collections;

public class Memory : MonoBehaviour
{
    private TimerScript timer;
    private TextMeshProUGUI memorytxt;

    [SerializeField] private float timeBonus = 1f; // Customizable extra time
    [SerializeField] private string memoryMessage = "You gained a memory!"; // Assignable message
    [SerializeField] private float messageDuration = 5f; // Time before text resets

    void Start()
    {
        timer = GameObject.Find("TimerUI")?.GetComponent<TimerScript>();
        memorytxt = GameObject.Find("MemoryText")?.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if Player touches this object
        {
            if (timer != null)
            {
                timer.AddTime(timeBonus); // Add the bonus time
                Debug.Log("🕒 Added " + timeBonus + " seconds!");
            }
            else
            {
                Debug.LogWarning("⚠️ No TimerScript found in the scene!");
            }

            if (memorytxt != null)
            {
                memorytxt.text = memoryMessage; // Update MemoryText before destroying
                MemoryTextManager.Instance.ResetTextAfterDelay(messageDuration); // Start reset countdown
            }
            else
            {
                Debug.LogWarning("⚠️ No MemoryText (TextMeshPro) found in the scene!");
            }

            Destroy(gameObject); // Remove this object after updating text
        }
    }
}
