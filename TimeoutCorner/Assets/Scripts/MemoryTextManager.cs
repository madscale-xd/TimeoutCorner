using UnityEngine;
using TMPro;
using System.Collections;

public class MemoryTextManager : MonoBehaviour
{
    public static MemoryTextManager Instance;
    private TextMeshProUGUI memorytxt;
    private Coroutine resetCoroutine; // Store the active coroutine

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        memorytxt = GetComponent<TextMeshProUGUI>();
    }

    public void ResetTextAfterDelay(float delay)
    {
        // If a reset coroutine is already running, stop it
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        // Start a new reset coroutine
        resetCoroutine = StartCoroutine(ResetTextCoroutine(delay));
    }

    private IEnumerator ResetTextCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (memorytxt != null)
        {
            memorytxt.text = ""; // Reset text
        }

        // Clear the reference after completion
        resetCoroutine = null;
    }
}
