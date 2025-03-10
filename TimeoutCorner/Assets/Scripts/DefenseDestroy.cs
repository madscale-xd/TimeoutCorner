using UnityEngine;

public class DefenseDestroy : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider[] colliders;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>(); // Get mesh
        colliders = GetComponents<Collider>(); // Get all colliders
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "WindWall") // Check name, NOT tag!
        {
            DisableObject(); // Instead of destroying, disable it
        }
    }

    void DisableObject()
    {
        meshRenderer.enabled = false; // Hide mesh
        foreach (Collider col in colliders)
        {
            col.enabled = false; // Disable colliders
        }
    }

    public void ReactivateObject()
    {
        meshRenderer.enabled = true; // Show mesh
        foreach (Collider col in colliders)
        {
            col.enabled = true; // Enable colliders
        }
    }
}
