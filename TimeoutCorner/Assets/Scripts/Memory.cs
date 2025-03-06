using UnityEngine;

public class Memory : MonoBehaviour
{
    private TimerScript timer;
    [SerializeField] private float timeBonus = 1f; // Customizable extra time

    void Update(){
        timer = GameObject.Find("TimerUI").GetComponent<TimerScript>();
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

            Destroy(gameObject); // Remove this object after giving time
        }
    }
}
