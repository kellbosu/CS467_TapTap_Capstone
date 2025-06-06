using TMPro;
using UnityEngine;

// ScoreManager handles displaying instant hit score text (Perfect, Good, Fine, Miss).
// Implemented as Singleton for global access.
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text hitScoreText;

    // Ensure singleton instance on object creation.
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Update hit score UI text.
    public void ShowHitScore(string score)
    {
        hitScoreText.text = score;
        Debug.Log("Hit score displayed: " + score);
    }
}
