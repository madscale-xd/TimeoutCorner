using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 lastCheckpoint;

    void Start()
    {
        lastCheckpoint = transform.position; // Start position is the first checkpoint
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpoint = checkpointPosition;
        Debug.Log("Checkpoint updated: " + lastCheckpoint);
    }

    public void Respawn()
    {
        transform.position = lastCheckpoint;
        Debug.Log("Player respawned at: " + lastCheckpoint);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            Respawn();
        }
    }
}
