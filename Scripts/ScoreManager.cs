using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TMP_Text hitScoreText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowHitScore(string score)
    {
        hitScoreText.text = score;
        Debug.Log("Hit score displayed: " + score);
    }
}
