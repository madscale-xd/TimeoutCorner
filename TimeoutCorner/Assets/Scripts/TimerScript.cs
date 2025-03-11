using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to a TextMeshPro UI element
    private GameObject player; // Reference to the player object

    public static float startTime = 13f; // Start from 6 seconds for Level 1, 11 for 2, 13 for boat
    public float remainingTime;
    private bool isRunning = true;
    private bool hasTriggeredEvent = false; // Prevents multiple triggers when time reaches 0

    private PlayerMovement playerMove;
    private PlayerRespawn playerRespawn;
    private MovableObjectsReset movReset;
    private MovingPlatformsReset movPReset;
    private ResetWardens resetWardens;

    void Start()
    {
        remainingTime = startTime;
        UpdateTimerText();
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerMove = player.GetComponent<PlayerMovement>();
        playerRespawn = player.GetComponent<PlayerRespawn>();
        resetWardens = player.GetComponent<ResetWardens>();
        movReset = player.GetComponent<MovableObjectsReset>();
        movPReset = player.GetComponent<MovingPlatformsReset>();
        if (isRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            remainingTime = Mathf.Max(remainingTime, 0); // Ensure it never goes below zero
            UpdateTimerText();

            if (remainingTime <= 0 && !hasTriggeredEvent)
            {
                hasTriggeredEvent = true;
                OnTimerEnd(); // Call the event when timer reaches zero
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        int milliseconds = Mathf.FloorToInt((remainingTime * 100) % 100);
        timerText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        playerMove.OnTimerResetPlayer();
        resetWardens.ReactivateWarden();
        remainingTime = startTime;
        hasTriggeredEvent = false;
        UpdateTimerText();
        GameObject boatObject = GameObject.Find("Boat");
            if (boatObject != null)
            {
                BoatScript boatScript = boatObject.GetComponent<BoatScript>();
                if (boatScript != null)
                {
                    boatScript.OnTimerReset();
                }
            }
            else
            {
                Debug.LogWarning("⚠️ Boat GameObject not found in the scene!");
            }
    }

    void OnTimerEnd()
    {
        Debug.Log("⏳ Timer ended! Respawning player...");
        movReset.ResetMovableObjects(); //ResetMovableObjects
        movPReset.ResetMovingPlatforms(); //ResetMovingPlatforms

        if (playerRespawn != null)
        {
            playerRespawn.Respawn(); // Call Respawn instead of destroying
        }
        else
        {
            Debug.LogWarning("⚠️ PlayerRespawn component is missing on player!");
        }

        ResetTimer();
    }

    public void AddTime(float extraTime)
    {
        startTime += extraTime;
        UpdateTimerText();
    }

}
