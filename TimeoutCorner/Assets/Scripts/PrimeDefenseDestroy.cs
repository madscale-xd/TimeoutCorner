using UnityEngine;

public class PrimeDefenseDestroy : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider[] colliders;
    private TimerScript timerScript;
    private SFXManager sfx;
    public AudioClip newMusic;
    private bool hasPlayedSFX = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>(); // Get mesh
        colliders = GetComponents<Collider>(); // Get all colliders
        GameObject timerObject = GameObject.Find("TimerUI");
        timerScript = timerObject.GetComponent<TimerScript>();
        sfx = timerObject.GetComponent<SFXManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovingPlatform") && TimerScript.GetStartTime()>=22) // Check name, NOT tag!
        {
            sfx.PlaySFX(7);
            timerScript.DisableTimer();
            BGMManager.Instance.PlayBGM(newMusic, 1.5f);
            Destroy(gameObject); // Instead of destroying, disable it
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MovingPlatform") && TimerScript.GetStartTime() >= 22)
        {
            if (!hasPlayedSFX) // Check if SFX has not been played yet
            {
                sfx.PlaySFX(7);
                hasPlayedSFX = true; // Mark as played to prevent repeats
            }

            timerScript.DisableTimer();
            BGMManager.Instance.PlayBGM(newMusic, 1.0f);
            Destroy(gameObject); // Instead of destroying, disable it
        }
    }
}
