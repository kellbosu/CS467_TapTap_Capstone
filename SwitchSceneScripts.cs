using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchSceneScripts : MonoBehaviour
{
    public void LoadScene3Rail()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadScene4Rail()
    {
        SceneManager.LoadScene(3);
    }
}
