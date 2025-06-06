using TMPro;
using UnityEngine;

// Manages combo count, score calculation, fever mode, and related UI updates.
// Handles both hit and miss events, and transitions between normal and fever modes.
public class ComboManager : MonoBehaviour
{
    public AudioClip feverSfx;
    public GameObject yaySprite; // Fever mode visual sprite.
    public GameObject missSprite; // Fever mode visual sprite.
    public TMP_Text comboText;
    public TMP_Text totalScoreText;

    public int totalComboCount = 0;
    public int totalScore = 0;

    public int perfectScore = 100;
    public int goodScore = 75;
    public int fineScore = 50;

    private int recoveryComboCount = 0;
    private bool isInMissState = false; // Whether player is in miss state

    public int comboHit = 0;
    public int maxHit = 0;

    public int comboForFever = 10; // Combo needed to trigger fever
    public int recoveryNeeded = 3; // Hits needed to recover after a miss

    // Initialize combo and score UI at game start.
    void Start()
    {
        comboText.text = "x0";
        totalScoreText.text = "0000000";
    }

    // Called when a perfect hit is registered.
    public void RegisterPerfect()
    {
        Debug.Log("Perfect");
        HandleHit();
    }

    // Called when a normal hit (good or fine) is registered.
    public void RegisterHit()
    {
        Debug.Log("Good");
        HandleHit();
    }

    // Called when a miss is registered. Updates miss state, combo, and fever exit.
    public void RegisterMiss()
    {
        Debug.Log("Miss");

        // Update maxHit if current comboHit is higher
        if (comboHit > maxHit)
        {
            maxHit = comboHit;
        }

        // Reset current combo after miss
        comboHit = 0;
        UpdateComboText();

        // Handle entering miss state visuals and recovery setup
        if (yaySprite.activeSelf)
        {
            yaySprite.SetActive(false);
            missSprite.SetActive(true);
            isInMissState = true;
            recoveryComboCount = 0;

            // Exit Fever Mode
            if (GameManager.Instance.isFeverMode)
            {
                GameManager.Instance.DeactivateFeverMode();
            }
        }
    }

    // Handles generic hit logic for both perfect and good hits.
    private void HandleHit()
    {
        // Increment combo counters
        totalComboCount++;
        comboHit++;
        UpdateComboText();

        // If in miss recovery state, count successful recoveries
        if (isInMissState)
        {
            recoveryComboCount++;

            if (recoveryComboCount >= recoveryNeeded)
            {
                missSprite.SetActive(false);
                isInMissState = false;
            }
        }

        // Trigger Fever Mode
        if (!GameManager.Instance.isFeverMode && !isInMissState && totalComboCount >= comboForFever)
        {
            Vector3 position = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(feverSfx, position, 3.0f);
            GameManager.Instance.ActivateFeverMode();
            yaySprite.SetActive(true);
        }
    }

    // Updates combo UI text display.
    private void UpdateComboText()
    {
        if (comboHit > 0)
        {
            comboText.text = $"x{comboHit}";
        }
        else
        {
            comboText.text = "x0";
        }
    }

    // Adds score for perfect hit and updates score UI.
    public void UpdatePerfect()
    {
        totalScore = totalScore + perfectScore;
        totalScoreText.text = totalScore.ToString("D7");
    }

    // Adds score for good hit and updates score UI.
    public void UpdateGood()
    {
        totalScore = totalScore + goodScore;
        totalScoreText.text = totalScore.ToString("D7");
    }

    // Adds score for fine hit and updates score UI.
    public void UpdateFine()
    {
        totalScore = totalScore + fineScore;
        totalScoreText.text = totalScore.ToString("D7");
    }
}
