using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHandler : MonoBehaviour
{
    // = (-1.47,-3.73,0)
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCorrectClicked)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;  

            Collider2D col = Physics2D.OverlapPoint(clickPosition);

            if (col != null && col.CompareTag("note"))
            {
                float distance = Vector2.Distance(transform.position, targetCircle.position);

                if (distance <= circleRange * triggerRange)
                {
                    isCorrectClicked = true;
                    int percentage = (int)(triggerRange * 100);
                    Debug.Log($"✅ Clicked note inside {percentage}% of target!");

                    //GetComponent<Animator>().enabled = false;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
                    noteAnimator.SetBool("isDisappear", true); 
                    noteBoomVfx.SetActive(true);                

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