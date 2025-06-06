using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// UIManager handles pause menu, result screen display,
// and updates final score display after music finishes.
public class UIManager : MonoBehaviour
{
    public bool musicFinished = false;

    // ---------- Public UI elements ----------
    public GameObject resultEvent;
    public GameObject pauseEvent;
    public TMP_Text finalResultText;

    private ComboManager comboManager; // comboManager reference.

    // Initialize ComboManager reference.
    void Start()
    {
        comboManager = FindObjectOfType<ComboManager>();
    }

    // Monitor whether music has finished, then show result screen.
    void Update()
    {
        if (musicFinished)
        {
            GameManager.Instance.WaitingGame();
            ResultEvent();
        }
    }

    // Trigger pause event (pause game and show pause menu).
    public void PauseEvent()
    {
        GameManager.Instance.PauseGame();
        pauseEvent.SetActive(true);
    }

    // Resume game when continue button is pressed.
    public void ContinueButton()
    {
        GameManager.Instance.ResumeGame();
        PauseClose();
    }

    // Stop game and show result screen directly when "Finish Run" button is pressed.
    public void FinishRunButton()
    {
        GameManager.Instance.WaitingGame();
        PauseClose();
        ResultEvent();
    }

    // Close pause UI.
    void PauseClose()
    {
        pauseEvent.SetActive(false);
    }

    // Display final result screen with score and max combo.
    void ResultEvent()
    {
        string finalScore = comboManager.totalScoreText.text;
        int maxHit = comboManager.maxHit;

        finalResultText.text = $"Final Score:\n{finalScore}\n\nMax Hit:\n{maxHit}";
        resultEvent.SetActive(true);
    }
}
