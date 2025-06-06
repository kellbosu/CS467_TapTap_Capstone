using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SceneSwitchManager handles switching between the 3-Rail and 4-Rail game modes.
// Triggered by menu UI button events.
public class SceneSwitchManager : MonoBehaviour
{
    // Load the 3-Rail mode scene (Scene index 2).
    public void LoadScene3Rail()
    {
        Debug.Log("Attempting to load 3-Rail scene (index 2)...");
        Time.timeScale = 1f;
        GameManager.Instance.StartGame();
        SceneManager.LoadScene(2);
    }

    // Load the 4-Rail mode scene (Scene index 3).
    public void LoadScene4Rail()
    {
        Debug.Log("Attempting to load 4-Rail scene (index 3)...");
        Time.timeScale = 1f;
        GameManager.Instance.StartGame();
        SceneManager.LoadScene(3);
    }
}
