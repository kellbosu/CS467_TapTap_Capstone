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

//                     GetComponent<Rigidbody2D>().velocity = Vector2.zero;  // Stop the note from moving
//                     Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
//                     noteAnimator.SetBool("isDisappear", true); // Trigger disappear animation
//                     noteBoomVfx.SetActive(true); // Activate boom visual effect               

//                 }
//                 else
//                 {
//                     comboManager.RegisterMiss();
//                     int percentage = (int)(triggerRange * 100);
//                 }
//             }else{
//                 comboManager.RegisterMiss();
//             }
//         }
//     }
// }

using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class NoteHandler : MonoBehaviour
{
    public Transform targetCircle;    

    

    //public AudioSource sfxSource;
    public AudioClip perfectSfx;
    public AudioClip goodSfx;
    public AudioClip fineSfx;

    public float perfectWindowMs = 50f;  
    public float goodWindowMs = 100f;
    public float fineWindowMs = 150f;

    public GameObject missUIPrefab;
    public GameObject perfectUI;
    public GameObject goodUI;
    public GameObject fineUI;


    public GameObject noteBoomVfx;

    private bool isCorrectClicked = false;
    private ComboManager comboManager;

    [HideInInspector] public double targetTimeMs; 
    [HideInInspector] public int column;  

    private Transform missUIParent;

    void Start()
    {
        comboManager = FindObjectOfType<ComboManager>();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(sceneIndex == 2)
        {
            missUIParent = GameObject.Find("Canvas").transform;
        }else if(sceneIndex == 3)
        {
            missUIParent = GameObject.Find("CanvasFourRail").transform;
        }
        //AudioSource sfxSource = GameObject.Find("sfxSource").GetComponent<AudioSource>();
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        if (!isCorrectClicked)
        {
            double currentTimeMs = AudioManager.Instance.GetDspTimeInMs();
            double lateWindowEnd = targetTimeMs + fineWindowMs;

            if (currentTimeMs > lateWindowEnd)
            {
                isCorrectClicked = true;
                comboManager.RegisterMiss();
               
                
                MissUI();

            }
        }

//------------------------------for mobile ---------------------      
        if (Input.GetMouseButtonDown(0) && !isCorrectClicked)
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;

            Collider2D col = Physics2D.OverlapPoint(clickPosition);

            if (col != null && col.gameObject == gameObject)
            {
                // float currentTimeMs = musicSource.time * 1000f;
                // float delta = Mathf.Abs(currentTimeMs - targetTimeMs);
                double currentTimeMs = AudioManager.Instance.GetDspTimeInMs();
                double delta = Mathf.Abs((float)(currentTimeMs - targetTimeMs));

                

                if (delta <= fineWindowMs)
                {
                    isCorrectClicked = true;
                    comboManager.RegisterHit();
                    //Debug.Log($"Clicked within {delta}ms of target time!");
                    Judge(delta);

                    Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
                    noteAnimator.SetBool("isDisappear", true);
                    noteBoomVfx.SetActive(true);
                }
                else
                {
                    comboManager.RegisterMiss();
                    MissUI();
                    Debug.Log($"Auto Miss! currentTime = {currentTimeMs}, targetTime = {targetTimeMs}");
                }
            }
        }
//------------------------------for mobile ---------------------      

//------------------------------for win/mac ---------------------      
        if (!isCorrectClicked && IsKeyForThisColumnPressed())
        {
            double currentTimeMs = AudioManager.Instance.GetDspTimeInMs();
            double delta = Math.Abs(currentTimeMs - targetTimeMs);
            
            //Judge(delta);


            if (delta <= fineWindowMs)
            {
                isCorrectClicked = true;
                comboManager.RegisterHit();
                //Debug.Log($"Key press hit on column {column} within {delta}ms");
                Judge(delta);

                Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
                noteAnimator.SetBool("isDisappear", true);
                noteBoomVfx.SetActive(true);
            }
        }
//------------------------------for win/mac ---------------------      


    }
            private bool IsKeyForThisColumnPressed()
        {
            switch (column)
            {
                case 1: return Input.GetKeyDown(KeyCode.A);
                case 2: return Input.GetKeyDown(KeyCode.S);
                case 3: return Input.GetKeyDown(KeyCode.D);
                case 4: return Input.GetKeyDown(KeyCode.F);
                default: return false;
            }
        }

        private void Judge(double delta)
    {
        if (delta <= perfectWindowMs)
        {
            Vector3 position = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(perfectSfx, position, 4.0f); 
            //isCorrectClicked = true;
            //comboManager.RegisterGood();  
            //totalScore = totalScore + perfectScore;
            comboManager.UpdatePerfect();
            Debug.Log($"Perfect! delta = {delta}ms");
            perfectUI.SetActive(true);
        }
        else if (delta <= goodWindowMs)
        {
            Vector3 position = Camera.main.transform.position; 
            AudioSource.PlayClipAtPoint(goodSfx, position, 5.0f); 
            //isCorrectClicked = true;
            //comboManager.RegisterGood();
            //totalScore = totalScore + goodScore;
            comboManager.UpdateGood();
            Debug.Log($"Good! delta = {delta}ms");
            goodUI.SetActive(true);
        }
        else if (delta <= fineWindowMs)
        {
            Vector3 position = Camera.main.transform.position; 
            AudioSource.PlayClipAtPoint(fineSfx, position, 70.0f); 
            //isCorrectClicked = true;
            //comboManager.RegisterGood();
            //totalScore = totalScore + fineScore;
            comboManager.UpdateFine();
            Debug.Log($"Fine! delta = {delta}ms");
            fineUI.SetActive(true);
        }
        
    }
    private void MissUI()
    {
        Vector3 spawnPos = Vector3.zero;

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if(sceneIndex == 2)
        {
            if (column == 1)
                spawnPos = new Vector3(-198f, -458f, 0f);
            else if (column == 2)
                spawnPos = new Vector3(48f, -458f, 0f);
            else if (column == 3)
                spawnPos = new Vector3(287f, -458f, 0f);
        }else if(sceneIndex == 3)
        {
            if (column == 1)
                spawnPos = new Vector3(-235f, -458f, 0f);
            else if (column == 2)
                spawnPos = new Vector3(-48f, -458f, 0f);
            else if (column == 3)
                spawnPos = new Vector3(143f, -458f, 0f);
            else if (column == 4)
                spawnPos = new Vector3(330f, -458f, 0f);
        }


        GameObject missUIInstance = Instantiate(missUIPrefab, missUIParent);
        missUIInstance.GetComponent<RectTransform>().localPosition = spawnPos;
        Destroy(missUIInstance, 1.0f);
    }

}
