using UnityEngine;
using TMPro;
using System.Collections;

public class MemoryTextManager : MonoBehaviour
{
    public static MemoryTextManager Instance;
    private TextMeshProUGUI memorytxt;
    private Coroutine resetCoroutine; // Store the active coroutine
    
    private TimerScript timer;
    private SFXManager sfx;

    void Awake()
    {
        timer = GameObject.Find("TimerUI")?.GetComponent<TimerScript>();
        sfx = timer.GetComponent<SFXManager>();

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
        sfx.PlaySFX(3);
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
