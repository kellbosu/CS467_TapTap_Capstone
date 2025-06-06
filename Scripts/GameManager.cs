using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager handles overall game state management including pause, resume,
// restart, fever mode, and scene transitions. Singleton pattern used.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Game state: Waiting, Playing, Paused
    public string currentGameState = "Waiting";
    public bool isFeverMode = false;

    // Initialize Singleton instance.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Ensure timescale is reset at game start.
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Set game to waiting state.
    public void WaitingGame()
    {
        currentGameState = "Waiting";
        isFeverMode = false;
        Debug.Log("Game Waiting");
    }

    // Start the game and begin playing music.
    public void StartGame()
    {
        currentGameState = "Playing";
        isFeverMode = false;
        AudioManager.Instance.PlayMusic();
        Debug.Log("Game Started");
    }

    // Pause the game and music.
    public void PauseGame()
    {
        AudioManager.Instance.PauseMusic();
        Time.timeScale = 0;
        currentGameState = "Paused";
    }

    // Resume the game and music.
    public void ResumeGame()
    {
        Time.timeScale = 1;
        currentGameState = "Playing";
        AudioManager.Instance.ResumeMusic();
    }

    // Activate Fever Mode.
    public void ActivateFeverMode()
    {
        isFeverMode = true;
        Debug.Log("Fever Mode Activated");
    }

    // Deactivate Fever Mode.
    public void DeactivateFeverMode()
    {
        isFeverMode = false;
        Debug.Log("Fever Mode Ended");
    }

    // Restart the current game scene and reset music.
    public void RestartGame()
    {
        Time.timeScale = 1f;

        // Clean up AudioManager to avoid duplicate instances
        if (AudioManager.Instance != null)
        {
            Destroy(AudioManager.Instance.gameObject);
        }

        // Reload current scene
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        StartGame();
        Debug.Log("Game Restarted");
    }

    // Return to main menu (scene 0) and reset game state.
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);

        // Clean up AudioManager
        if (AudioManager.Instance != null)
        {
            Destroy(AudioManager.Instance.gameObject);
        }

        WaitingGame();
        Debug.Log("Load Main Menu");
    }
}
