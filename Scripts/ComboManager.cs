using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public GameObject yaySprite;
    public GameObject missSprite;

    private int totalComboCount = 0;
    private int recoveryComboCount = 0;
    private bool isInMissState = false;

    public int comboForFever = 10;   // Combo needed to trigger fever
    public int recoveryNeeded = 3;   // Hits needed to recover after a miss

    public void RegisterPerfect()
    {
        Debug.Log("Perfect");
        HandleHit();
    }

    public void RegisterGood()
    {
        Debug.Log("Good");
        HandleHit();
    }

    public void RegisterMiss()
    {
        Debug.Log("Miss");

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
            GameManager.Instance.ActivateFeverMode();
            yaySprite.SetActive(true); 
        }
    }
}
