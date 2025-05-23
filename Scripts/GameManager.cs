using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Waiting
    //Playing
    //Paused
    public string currentGameState = "Waiting";
    public AudioSource musicSource; 

    public bool isFeverMode = false;

    void Awake()
    {
        // Instance
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
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void WaitingGame()
    {
        currentGameState = "Waiting";
        isFeverMode = false;
        Debug.Log("Game Waiting");
    }

    public void StartGame()
    {
        currentGameState = "Playing";
        isFeverMode = false;
        Debug.Log("Game Started");
    }
    public void PauseGame()
    {
                if (musicSource.isPlaying)
            musicSource.Pause();
        
        Time.timeScale = 0;
currentGameState = "Paused";
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        currentGameState = "Playing";
        Time.timeScale = 1;
        musicSource.UnPause();
        Debug.Log("Game Resumed");
    }

    public void ActivateFeverMode()
    {
        isFeverMode = true;
        Debug.Log("Fever Mode Activated");
    }

    public void DeactivateFeverMode()
    {
        isFeverMode = false;
        Debug.Log("Fever Mode Ended");
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        StartGame();
        Debug.Log("Game Restarted");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        WaitingGame();
        Debug.Log("Load Main Menu");
    }
}
