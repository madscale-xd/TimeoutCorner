using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
    [SerializeField] private string endSceneName = "EndMenu"; // Assignable in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the Player collides
        {
            TimerScript timer = FindObjectOfType<TimerScript>(); // Find existing timer
            if (timer != null)
            {
                Destroy(timer.gameObject); // Destroy it before loading the new scene
            }
            Debug.Log("🎉 Game Over! Loading End Menu...");
            SceneManager.LoadScene(endSceneName); // Load EndMenu scene
        }
    }
}
