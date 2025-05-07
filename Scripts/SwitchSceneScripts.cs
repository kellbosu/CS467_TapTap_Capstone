using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// This script switches between the 3-Rail and 4-Rail game mode scenes.
public class SwitchSceneScripts : MonoBehaviour
{
    // Loads the scene with index 4
    public void LoadScene3Rail()
    {
        Debug.Log("Attempting to load 3-Rail scene (index 4)...");
        SceneManager.LoadScene(4);
    }

    // Loads the scene with index 3
    public void LoadScene4Rail()
    {
        Debug.Log("Attempting to load 4-Rail scene (index 3)...");
        SceneManager.LoadScene(3);
    }
}
