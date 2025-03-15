using UnityEngine;
using System.Collections.Generic;

public class MovableObjectsReset : MonoBehaviour
{
    private Dictionary<GameObject, Vector3> initialPositions = new Dictionary<GameObject, Vector3>();
    private TriggerActivator trigger;

    void Start()
    {
        trigger = GameObject.Find("DefenseTrigger").GetComponent<TriggerActivator>();

        // Store initial positions at start
        GameObject[] movableObjects = GameObject.FindGameObjectsWithTag("Movable");
        foreach (GameObject obj in movableObjects)
        {
            if (!initialPositions.ContainsKey(obj))
            {
                initialPositions[obj] = obj.transform.position;
            }
        }
    }

    void Update()
    {
        // Track newly spawned Movable objects
        GameObject[] movableObjects = GameObject.FindGameObjectsWithTag("Movable");

        foreach (GameObject obj in movableObjects)
        {
            if (!initialPositions.ContainsKey(obj) && obj.activeInHierarchy) // Only track if active
            {
                initialPositions[obj] = obj.transform.position;
            }
        }
    }

    public void ResetMovableObjects()
    {
        trigger.StopMovement();

        foreach (var entry in initialPositions)
        {
            if (entry.Key != null && entry.Key.activeInHierarchy) // Ensure object still exists and is active
            {
                // Reset position
                entry.Key.transform.position = entry.Value;

                // Reset velocity if Rigidbody exists
                Rigidbody rb = entry.Key.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = false;  // ✅ Ensure it's not stuck
                    rb.WakeUp();  // ✅ Make sure physics reactivates
                }
            }
        }

        Debug.Log("✅ Movable objects reset and ready to move again.");
    }

}
