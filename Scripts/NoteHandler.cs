usinusing System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHandler : MonoBehaviour
{
    public Transform targetCircle;
    private float circleRange;
    public float triggerRange;
    private bool isCorrectClicked = false;
    private bool hasBeenClicked = false;
    public GameObject noteBoomVfx;
    public float noteSpeed = 0.2f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.down * noteSpeed;
        circleRange = GetComponent<CircleCollider2D>().radius * transform.localScale.x * 1.5f;
        // Delayed call to handle unclicked notes
        // More forgiving on the player and makes more fun
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCorrectClicked)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;

            Collider2D col = Physics2D.OverlapPoint(clickPosition);

            if (col != null && col.CompareTag("note") && col.gameObject == this.gameObject)
            {
                hasBeenClicked = true;

                float distance = Vector2.Distance(transform.position, targetCircle.position);
                float perfectRange = circleRange * 0.2f;
                float goodRange = circleRange * 0.5f;

                if (distance <= perfectRange)
                {
                    ScoreManager.Instance.ShowHitScore("Perfect!");;
                    TriggerNote();
                }
                else if (distance <= goodRange)
                {
                    ScoreManager.Instance.ShowHitScore("Good!");
                    TriggerNote();
                }
                else
                {
                    ScoreManager.Instance.ShowHitScore("Miss!");
                    isCorrectClicked = true;
                }
            }
        }
    }

    void HandleUnclickedNote()
    {
        if (!hasBeenClicked && !isCorrectClicked)
        {
            ScoreManager.Instance.ShowHitScore("Miss!");
        }

        Destroy(gameObject);
    }

    void TriggerNote()
    {
        isCorrectClicked = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
        noteAnimator.SetBool("isDisappear", true);
        noteBoomVfx.SetActive(true);
    }
// Triggers miss if notes are not hit at all
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("missZone") && !isCorrectClicked)
        {
            ScoreManager.Instance.ShowHitScore("Miss!");
            isCorrectClicked = true;
            Destroy(gameObject);
        }
    }
}