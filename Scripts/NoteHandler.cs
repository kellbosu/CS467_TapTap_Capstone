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
            float perfectRange = circleRange * 0.2f;
            float goodRange = circleRange * 0.5f;

            if (distance <= perfectRange)
            {
                Debug.Log("🎯 Perfect!");
                TriggerNote();
            }
            else if (distance <= goodRange)
            {
                Debug.Log("👍 Good!");
                TriggerNote();
            }
            else
            {
                Debug.Log("❌ Miss!");
            }
        }
    }
}

void TriggerNote()
{
    isCorrectClicked = true;
    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
    noteAnimator.SetBool("isDisappear", true);
    noteBoomVfx.SetActive(true);
}
}