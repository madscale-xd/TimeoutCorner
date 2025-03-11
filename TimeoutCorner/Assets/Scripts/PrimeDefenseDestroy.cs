using UnityEngine;

public class PrimeDefenseDestroy : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider[] colliders;
    private TimerScript timerScript;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>(); // Get mesh
        colliders = GetComponents<Collider>(); // Get all colliders
        GameObject timerObject = GameObject.Find("TimerUI");
        timerScript = timerObject.GetComponent<TimerScript>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform")) // Check name, NOT tag!
        {
            timerScript.DisableTimer();
            Destroy(gameObject); // Instead of destroying, disable it
        }
    }
}
