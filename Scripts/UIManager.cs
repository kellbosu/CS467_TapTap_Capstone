using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public bool musicFinished = false;
    public GameObject resultEvent;
    public GameObject pauseEvent;
    public TMP_Text finalResultText;
    private ComboManager comboManager;
    // Start is called before the first frame update
    void Start()
    {
        comboManager = FindObjectOfType<ComboManager>();
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
        string finalScore = comboManager.totalScoreText.text; 
        int maxHit = comboManager.maxHit;

        finalResultText.text = $"Final Score:\n{finalScore}\n\nMax Hit:\n{maxHit}";
        resultEvent.SetActive(true);
    }
}
