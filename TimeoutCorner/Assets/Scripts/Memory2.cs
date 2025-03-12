using UnityEngine;
using TMPro;
using System.Collections;

public class Memory2 : MonoBehaviour
{
    private TextMeshProUGUI memorytxt;

    [SerializeField] private string memoryMessage = "You gained a memory!"; // Assignable message
    [SerializeField] private float messageDuration = 5f; // Time before text resets

    void Start()
    {
        memorytxt = GameObject.Find("MemoryText")?.GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if Player touches this object
        {
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
