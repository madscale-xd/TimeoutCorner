using UnityEngine;
using System.Collections.Generic;

public class MovingPlatformsReset : MonoBehaviour
{
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();

    void Update()
    {
        // Continuously track newly spawned "Movable" objects
        GameObject[] movingPlatforms = GameObject.FindGameObjectsWithTag("MovingPlatform");

        foreach (GameObject obj in movingPlatforms)
        {
            if (!initialPositions.ContainsKey(obj)) // Only store if not already tracked
            {
                initialPositions[obj] = obj.transform.position;
            }
        }
    }

    public void ResetMovingPlatforms()
    {
        foreach (var entry in initialPositions)
        {
            if (entry.Key != null) // Ensure object still exists
            {
                // Reset position
                entry.Key.transform.position = entry.Value;

                // Remove velocity if Rigidbody exists
                Rigidbody rb = entry.Key.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }

        Debug.Log("All Moving Platforms reset to initial positions with zero velocity.");
    }
}
