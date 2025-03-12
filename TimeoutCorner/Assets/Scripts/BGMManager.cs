using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance; // Singleton instance

    [Header("Audio Source")]
    public AudioSource bgmSource; // Assigned in the Inspector

    [Header("Background Music Clips")]
    public AudioClip defaultBGM; // Default background music

    private Coroutine fadeCoroutine; // To handle fade-in/out transitions

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep BGMManager across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (bgmSource == null)
        {
            Debug.LogError("⚠️ BGMManager: No AudioSource assigned!");
            return;
        }

        if (defaultBGM != null)
        {
            PlayBGM(defaultBGM); // Start playing default music
        }
    }

    /// <summary>
    /// Plays a new BGM with optional fading transition.
    /// </summary>
    public void PlayBGM(AudioClip newBGM, float fadeDuration = 1f)
    {
        if (bgmSource.clip == newBGM) return; // Don't restart the same track

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeBGM(newBGM, fadeDuration));
    }

    /// <summary>
    /// Fades out the current music and plays a new one.
    /// </summary>
    private IEnumerator FadeBGM(AudioClip newBGM, float duration)
    {
        float startVolume = bgmSource.volume;

        // Fade out
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newBGM;
        bgmSource.Play();
        bgmSource.loop = true;

        // Fade in
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, startVolume, t / duration);
            yield return null;
        }

        bgmSource.volume = startVolume; // Ensure final volume is correct
    }

    /// <summary>
    /// Stops the current BGM with a fade-out.
    /// </summary>
    public void StopBGM(float fadeDuration = 1f)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeOut(float duration)
    {
        float startVolume = bgmSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        bgmSource.Stop();
    }
}
