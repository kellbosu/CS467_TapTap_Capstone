using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    public AudioClip feverSfx;
    public GameObject yaySprite;
    public GameObject missSprite;
    public TMP_Text comboText;
    public TMP_Text totalScoreText;

    public int totalComboCount = 0;
    public int totalScore = 0;
    public int perfectScore = 100;
    public int goodScore = 75;
    public int fineScore = 50;

    private int recoveryComboCount = 0;
    private bool isInMissState = false;

    public int comboHit = 0;
    public int maxHit = 0;

    public int comboForFever = 10;   // Combo needed to trigger fever
    public int recoveryNeeded = 3;   // Hits needed to recover after a miss

    void Start()
    {
        comboText.text = "x0";
        totalScoreText.text = "0000000";

    }
    public void RegisterPerfect()
    {
        Debug.Log("Perfect");
        HandleHit();
    }

    public void RegisterHit()
    {
        Debug.Log("Good");
        HandleHit();
    }

    public void RegisterMiss()
    {
        Debug.Log("Miss");
        if(comboHit>maxHit)
        {
            maxHit = comboHit;
        }
        comboHit = 0;
        UpdateComboText();

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

    private void HandleHit()
    {
        totalComboCount++;
        comboHit++;
        UpdateComboText();

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
        if (!GameManager.Instance.isFeverMode &&
            !isInMissState &&
            totalComboCount >= comboForFever)
        {
            Vector3 position = Camera.main.transform.position; 
            AudioSource.PlayClipAtPoint(feverSfx, position, 3.0f); 
            GameManager.Instance.ActivateFeverMode();
            yaySprite.SetActive(true); 
        }
    }

    private void UpdateComboText()
    {
        if (comboHit > 0)
        {comboText.text = $"x{comboHit}";}
        else
        {comboText.text = "x0"; }
    }

    public void UpdatePerfect()
    {
        totalScore =totalScore + perfectScore;
        totalScoreText.text = totalScore.ToString("D7");
    }

    public void UpdateGood()
    {
        totalScore= totalScore + goodScore;
        totalScoreText.text = totalScore.ToString("D7");
    }

    public void UpdateFine()
    {
        totalScore = totalScore + fineScore;
        totalScoreText.text = totalScore.ToString("D7");
    }
}
