using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// StartSceneController handles detecting first player input on the start screen
// and transitions into the main menu (Scene index 1).
// Supports both mobile (touch) and desktop (mouse click).
public class StartSceneController : MonoBehaviour
{
    void Update()
    {
        // Check for touch input (mobile)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("Touch detected. Loading next scene...");
            GameManager.Instance.WaitingGame();
            SceneManager.LoadScene(1);
        }

        // For testing in Editor with mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click detected. Loading next scene...");
            GameManager.Instance.WaitingGame();
            SceneManager.LoadScene(1);
        }
    }
}
