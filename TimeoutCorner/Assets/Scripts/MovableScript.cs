using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class MovableScript : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip moveSound; // Assignable in Inspector
    [Range(0f, 1f)] public float volume = 1f;
    public float fadeOutDuration = 0.5f; // Time it takes to fade out the sound
    public float lingerDuration = 0.3f; // How long the sound lingers before fading

    [Header("Physics Settings")]
    public float stopThreshold = 0.05f; // Speed threshold to consider the object "stopped"

    private Rigidbody rb;
    private AudioSource audioSource;
    private bool playerPushing = false;
    private Coroutine fadeOutCoroutine;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Helps smooth movement
        
        // Set up AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = moveSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0; // Start silent
    }

    void FixedUpdate()
    {
        bool isMoving = rb.velocity.magnitude > stopThreshold; // Object is still moving?

        if (playerPushing && isMoving)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = volume; // Ensure volume is up before playing
                audioSource.Play();
            }

            // Cancel fade-out if it's about to start
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
                fadeOutCoroutine = null;
                audioSource.volume = volume; // Reset volume immediately
            }
        }
        else if (!isMoving) // Object has stopped moving completely
        {
            // Start fade-out after linger time
            if (audioSource.isPlaying && fadeOutCoroutine == null)
            {
                fadeOutCoroutine = StartCoroutine(LingerThenFadeOut());
            }
        }
    }

    private void OnCollisionStay(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            playerPushing = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerPushing = false;
        }
    }

    private IEnumerator LingerThenFadeOut()
    {
        yield return new WaitForSeconds(lingerDuration); // Linger before fading

        float startVolume = audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeOutDuration);
            yield return null;

            // If pushing starts again, cancel the fade-out
            if (playerPushing)
            {
                audioSource.volume = volume; // Reset volume immediately
                yield break;
            }
        }

        audioSource.Stop();
        audioSource.volume = volume; // Reset volume for next push
        fadeOutCoroutine = null; // Reset coroutine tracker
    }
}
