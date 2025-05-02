using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Handles note falling and click detection in rhythm game.
public class NoteHandler : MonoBehaviour
{
    public Transform targetCircle;    
    private float circleRange;    
    public float triggerRange;
    private bool isCorrectClicked;
    public GameObject noteBoomVfx;
    public float noteSpeed = 0.2f;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.down * noteSpeed;
        circleRange = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
        Destroy(gameObject, 10f);
    }
    
    /// Checks for mouse click, validates hit, and triggers animation/VFX.
    void Update()
    {
        // Detect left-click and ensure the note hasn't already been clicked
        if (Input.GetMouseButtonDown(0) && !isCorrectClicked)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;  

            Collider2D col = Physics2D.OverlapPoint(clickPosition);

            // Check if click is within valid trigger range
            if (col != null && col.CompareTag("note"))
            {   
                // Measure distance from note to target circle
                float distance = Vector2.Distance(transform.position, targetCircle.position);

                if (distance <= circleRange * triggerRange)
                {
                    isCorrectClicked = true;
                    int percentage = (int)(triggerRange * 100);
                    Debug.Log($"✅ Clicked note inside {percentage}% of target!");

                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;  // Stop the note from moving
                    Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
                    noteAnimator.SetBool("isDisappear", true); // Trigger disappear animation
                    noteBoomVfx.SetActive(true); // Activate boom visual effect               

                }
                else
                {
                    int percentage = (int)(triggerRange * 100);
                    Debug.Log($"❌ Clicked note but outside {percentage}% area, destroy after 5s.");
                }
            }
        }
    }
}