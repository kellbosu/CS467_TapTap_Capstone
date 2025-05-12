using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// This script detects the first screen touch or mouse click
/// and loads the next scene (Scene index 1).
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
