using UnityEngine;
using UnityEngine.SceneManagement;  // To load the menu scene
using UnityEngine.UI;              // For UI button functionality

public class PauseView : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;
    
    private PlayerMovement playerMovement;
    private FirstPersonLook fpl;
    private SFXManager sfxManager; // Reference to SFXManager
    private bool isPaused = false;

    void Start()
    {
        pauseMenuCanvas.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        menuButton.onClick.AddListener(LoadMenu);

        sfxManager = FindObjectOfType<SFXManager>(); // Find the SFX Manager
        Cursor.visible = false;
    }

    void Update()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        fpl = GameObject.Find("Head").GetComponent<FirstPersonLook>();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Cursor.visible = false;
                ResumeGame();
            }
            else
            {
                Cursor.visible = true;
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0;

        if (playerMovement != null) playerMovement.enabled = false;
        if (fpl != null) fpl.enabled = false;

        Cursor.visible = true;
        isPaused = true;

        // Mute only SFX, keep BGM playing
        if (sfxManager != null)
        {
            sfxManager.MuteAllSFX();
        }
    }

    void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1;

        if (playerMovement != null) playerMovement.enabled = true;
        if (fpl != null) fpl.enabled = true;

        Cursor.visible = false;
        isPaused = false;

        // Unmute SFX
        if (sfxManager != null)
        {
            sfxManager.UnmuteAllSFX();
        }
    }

    void LoadMenu()
    {
        Time.timeScale = 1;
        TimerScript timer = FindObjectOfType<TimerScript>();
        if (timer != null)
        {
            Destroy(timer.gameObject);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
