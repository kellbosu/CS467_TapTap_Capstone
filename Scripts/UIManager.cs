using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool musicFinished = false;
    public GameObject resultEvent;
    public GameObject pauseEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (musicFinished)
        {   
            GameManager.Instance.WaitingGame();
            ResultEvent();
        }
    }
    public void PauseEvent()
    {
        GameManager.Instance.PauseGame();
        pauseEvent.SetActive(true);
    }
    public void ContinueButton()
    {
        GameManager.Instance.ResumeGame();
        PauseClose();
    }
    public void FinishRunButton()
    {
        GameManager.Instance.WaitingGame();
        PauseClose();
        ResultEvent();
    }
    void PauseClose()
    {
        pauseEvent.SetActive(false);
    }
    void ResultEvent()
    {
        resultEvent.SetActive(true);
    }
}
