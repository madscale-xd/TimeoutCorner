using UnityEngine;
using UnityEngine.SceneManagement;  // To load the menu scene
using UnityEngine.UI;              // For UI button functionality

public class PauseView : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuCanvas;  // Reference to the Pause Menu Canvas (assign in inspector)
    [SerializeField] private Button resumeButton;         // Reference to the Resume Button (assign in inspector)
    [SerializeField] private Button menuButton;           // Reference to the Menu Button (assign in inspector)

    private PlayerMovement playerMovement;
    private FirstPersonLook fpl;

    private bool isPaused = false;

    void Start()
    {
        // Initially, set the pause menu canvas to inactive
        pauseMenuCanvas.SetActive(false);

        // Set up button listeners
        resumeButton.onClick.AddListener(ResumeGame);
        menuButton.onClick.AddListener(LoadMenu);
        
        // Hide the cursor initially
        Cursor.visible = false;
    }

    void Update()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        fpl = GameObject.Find("Head").GetComponent<FirstPersonLook>();


        // Listen for Escape key to toggle pause state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Cursor.visible = false;
                ResumeGame();  // If paused, resume the game
            }
            else
            {
                Cursor.visible = true;
                PauseGame();   // If not paused, pause the game
            }
        }
    }

    void PauseGame()
    {
        // Activate the pause menu canvas and make it visible
        pauseMenuCanvas.SetActive(true);

        // Pause the game
        Time.timeScale = 0;

        // Disable PlayerMovement and firstpersonlook
        if (playerMovement != null) playerMovement.enabled = false;
        if (fpl != null) fpl.enabled = false;


        // Show the cursor
        Cursor.visible = true;

        // Set isPaused to true
        isPaused = true;
    }

    void ResumeGame()
    {
        // Deactivate the pause menu canvas
        pauseMenuCanvas.SetActive(false);

        // Resume the game
        Time.timeScale = 1;

        // Enable PlayerMovement and firstpersonlook
        if (playerMovement != null) playerMovement.enabled = true;
        if (fpl != null) fpl.enabled = true;

        // Hide the cursor
        Cursor.visible = false;

        // Set isPaused to false
        isPaused = false;
    }

    void LoadMenu()
    {
        Time.timeScale = 1;
        TimerScript timer = FindObjectOfType<TimerScript>(); // Find existing timer
        if (timer != null)
        {
            Destroy(timer.gameObject); // Destroy it before loading the new scene
        }

        SceneManager.LoadScene("MainMenu");
    }
}