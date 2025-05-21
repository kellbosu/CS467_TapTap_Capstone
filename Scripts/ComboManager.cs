using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public GameObject yaySprite;
    public GameObject missSprite;

    private int totalComboCount = 0;
    private int recoveryComboCount = 0;
    private int postMissHitStreak = 0;
    private bool isInMissState = false;

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
        }

        missSprite.SetActive(true);
        isInMissState = true;
        recoveryComboCount = 0;
        postMissHitStreak = 0;
        totalComboCount = 0;
    }

    private void HandleHit()
    {
        totalComboCount++;

        if (isInMissState)
        {
            recoveryComboCount++;
            postMissHitStreak++;

            if (recoveryComboCount >= 3)
            {
                missSprite.SetActive(false);
                isInMissState = false;
            }
        }
        else
        {
            postMissHitStreak++;
        }

        
        if (!yaySprite.activeSelf && !isInMissState && postMissHitStreak >= 10)
        {
            yaySprite.SetActive(true);
        }
    }
}
