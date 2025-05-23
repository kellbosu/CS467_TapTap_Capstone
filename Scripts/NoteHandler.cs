// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// /// Handles note falling and click detection in rhythm game.
// public class NoteHandler : MonoBehaviour
// {
//     public Transform targetCircle;    
//     private float circleRange;    
//     public float triggerRange;
//     private bool isCorrectClicked;
//     public GameObject noteBoomVfx;
//     public float noteSpeed = 0.2f;
//     ComboManager comboManager;
//     void Start()
//     {
//         comboManager = FindObjectOfType<ComboManager>();

//         GetComponent<Rigidbody2D>().velocity = Vector2.down * noteSpeed;
//         circleRange = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
//         Destroy(gameObject, 10f);
//     }
    
//     /// Checks for mouse click, validates hit, and triggers animation/VFX.
//     void Update()
//     {
//         // Detect left-click and ensure the note hasn't already been clicked
//         if (Input.GetMouseButtonDown(0) && !isCorrectClicked)
//         {
//             Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             clickPosition.z = 0;  

//             Collider2D col = Physics2D.OverlapPoint(clickPosition);

//             // Check if click is within valid trigger range
//             if (col != null && col.CompareTag("note"))
//             {   
//                 // Measure distance from note to target circle
//                 float distance = Vector2.Distance(transform.position, targetCircle.position);

//                 if (distance <= circleRange * triggerRange)
//                 {
//                     comboManager.RegisterGood();
//                     isCorrectClicked = true;
//                     int percentage = (int)(triggerRange * 100);
//                     Debug.Log($"✅ Clicked note inside {percentage}% of target!");

//                     GetComponent<Rigidbody2D>().velocity = Vector2.zero;  // Stop the note from moving
//                     Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
//                     noteAnimator.SetBool("isDisappear", true); // Trigger disappear animation
//                     noteBoomVfx.SetActive(true); // Activate boom visual effect               

//                 }
//                 else
//                 {
//                     comboManager.RegisterMiss();
//                     int percentage = (int)(triggerRange * 100);
//                     Debug.Log($"❌ Clicked note but outside {percentage}% area, destroy after 5s.");
//                 }
//             }else{
//                 comboManager.RegisterMiss();
//             }
//         }
//     }
// }

using UnityEngine;

/// Handles note falling and click detection in rhythm game.
public class NoteHandler : MonoBehaviour
{
    public Transform targetCircle;    
    public float allowedHitWindowMs = 150f; 
    public GameObject noteBoomVfx;

    private bool isCorrectClicked = false;
    private ComboManager comboManager;

    [HideInInspector] public float targetTimeMs; 
    public AudioSource musicSource; 

    void Start()
    {
        comboManager = FindObjectOfType<ComboManager>();
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCorrectClicked)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;

            Collider2D col = Physics2D.OverlapPoint(clickPosition);

            if (col != null && col.gameObject == gameObject)
            {
                float currentTimeMs = musicSource.time * 1000f;
                float delta = Mathf.Abs(currentTimeMs - targetTimeMs);

                if (delta <= allowedHitWindowMs)
                {
                    isCorrectClicked = true;
                    comboManager.RegisterGood();
                    Debug.Log($"Clicked within {delta}ms of target time!");

                    Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
                    noteAnimator.SetBool("isDisappear", true);
                    noteBoomVfx.SetActive(true);
                }
                else
                {
                    comboManager.RegisterMiss();
                    Debug.Log($"Clicked note too early/late: delta = {delta}ms");
                }
            }
        }
    }
}
