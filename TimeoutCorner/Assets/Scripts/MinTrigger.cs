using UnityEngine;

public class MinTrigger : MonoBehaviour
{
    [Header("Tag to Detect")]
    public string requiredTag = "ClockhandMin"; // Change this for different triggers

    private bool isObjectInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            isObjectInside = true;
            Debug.Log($"✅ {other.name} entered {gameObject.name}");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            isObjectInside = true; // Ensures it remains true while inside
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(requiredTag))
        {
            isObjectInside = false;
            Debug.Log($"❌ {other.name} exited {gameObject.name}");
        }
    }

    // Method to check if the required object is inside the trigger
    public bool IsObjectInside()
    {
        return isObjectInside;
    }
}
