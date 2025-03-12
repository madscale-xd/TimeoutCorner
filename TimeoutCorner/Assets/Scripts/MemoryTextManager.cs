using UnityEngine;
using TMPro;
using System.Collections;

public class MemoryTextManager : MonoBehaviour
{
    public static MemoryTextManager Instance;
    private TextMeshProUGUI memorytxt;

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
        StartCoroutine(ResetTextCoroutine(delay));
    }

    private IEnumerator ResetTextCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (memorytxt != null)
        {
            memorytxt.text = ""; // Reset text
        }
    }
}
