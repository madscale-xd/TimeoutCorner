using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public Transform checkpointArea; // The actual respawn point

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && checkpointArea != null) // Ensure the reference exists
        {
            other.GetComponent<PlayerRespawn>().SetCheckpoint(checkpointArea.position);
        }
    }
}
