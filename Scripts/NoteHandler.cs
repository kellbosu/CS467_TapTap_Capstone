using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles individual note behavior: click/key detection, hit judgment, combo update,
// feedback UI display, and automatic miss detection based on timing window.
public class NoteHandler : MonoBehaviour
{
    public Transform targetCircle;

    // ---------- Audio Clips for Judgment Feedback ----------
    public AudioClip perfectSfx;
    public AudioClip goodSfx;
    public AudioClip fineSfx;

    // ---------- Timing Windows for Judgment----------
    public float perfectWindowMs = 50f;
    public float goodWindowMs = 100f;
    public float fineWindowMs = 150f;

    // ---------- UI Elements for Hit Feedback ----------
    public GameObject missUIPrefab;
    public GameObject perfectUI;
    public GameObject goodUI;
    public GameObject fineUI;

    public GameObject noteBoomVfx;

    private bool isCorrectClicked = false;
    private ComboManager comboManager;

    [HideInInspector]
    public double targetTimeMs; // Absolute target hit time

    [HideInInspector]
    public int column; // Column this note belongs to

    private Transform missUIParent;

    // Initialization: locate ComboManager, assign correct canvas for miss UI display.
    void Start()
    {
        comboManager = FindObjectOfType<ComboManager>();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Assign different canvas for 3-rail or 4-rail mode
        if (sceneIndex == 2)
        {
            missUIParent = GameObject.Find("Canvas").transform;
        }
        else if (sceneIndex == 3)
        {
            missUIParent = GameObject.Find("CanvasFourRail").transform;
        }

        Destroy(gameObject, 10f); // Auto destroy note after lifetime
    }

    // Main loop to detect miss, player inputs, and handle hit logic.
    void Update()
    {
        // Handle auto-miss if note passes its judgment window
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

        //--------------------------------- For mobile input (tap) ---------------------------------
        if (Input.GetMouseButtonDown(0) && !isCorrectClicked)
        {
            // Convert screen click position to world coordinates
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0;

            // Check if the click overlaps with this note's collider
            Collider2D col = Physics2D.OverlapPoint(clickPosition);

            if (col != null && col.gameObject == gameObject)
            {
                double currentTimeMs = AudioManager.Instance.GetDspTimeInMs();
                double delta = Mathf.Abs((float)(currentTimeMs - targetTimeMs));

                // Handle valid hit
                if (delta <= fineWindowMs)
                {
                    isCorrectClicked = true;
                    comboManager.RegisterHit();
                    Judge(delta);
                    TriggerNoteDisappear();
                }
                else
                {
                    // Handle auto miss
                    comboManager.RegisterMiss();
                    MissUI();
                    Debug.Log(
                        $"Auto Miss! currentTime = {currentTimeMs}, targetTime = {targetTimeMs}"
                    );
                }
            }
        }
        //--------------------------------- For mobile input ---------------------------------

        //--------------------------------- For desktop input (keyboard) ---------------------------------
        if (!isCorrectClicked && IsKeyForThisColumnPressed())
        {
            double currentTimeMs = AudioManager.Instance.GetDspTimeInMs();
            double delta = Math.Abs(currentTimeMs - targetTimeMs);

            // If input is within judgment, register as hit
            if (delta <= fineWindowMs)
            {
                isCorrectClicked = true;
                comboManager.RegisterHit();
                Judge(delta);
                TriggerNoteDisappear();
            }
        }
        //--------------------------------- For desktop input ---------------------------------
    }

    // Detects which keyboard key belongs to which column.
    private bool IsKeyForThisColumnPressed()
    {
        switch (column)
        {
            case 1:
                return Input.GetKeyDown(KeyCode.A);
            case 2:
                return Input.GetKeyDown(KeyCode.S);
            case 3:
                return Input.GetKeyDown(KeyCode.D);
            case 4:
                return Input.GetKeyDown(KeyCode.F);
            default:
                return false;
        }
    }

    // Perform judgment based on hit delta.
    private void Judge(double delta)
    {
        // Perfect judgment
        if (delta <= perfectWindowMs)
        {
            Vector3 position = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(perfectSfx, position, 4.0f);
            comboManager.UpdatePerfect();
            Debug.Log($"Perfect! delta = {delta}ms");
            perfectUI.SetActive(true);
        }
        // Good judgment
        else if (delta <= goodWindowMs)
        {
            Vector3 position = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(goodSfx, position, 5.0f);
            comboManager.UpdateGood();
            Debug.Log($"Good! delta = {delta}ms");
            goodUI.SetActive(true);
        }
        // Fine judgment
        else if (delta <= fineWindowMs)
        {
            Vector3 position = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(fineSfx, position, 70.0f);
            comboManager.UpdateFine();
            Debug.Log($"Fine! delta = {delta}ms");
            fineUI.SetActive(true);
        }
    }

    // Play disappearance animation and activate VFX after successful hit.
    private void TriggerNoteDisappear()
    {
        Animator noteAnimator = transform.GetChild(0).GetComponent<Animator>();
        noteAnimator.SetBool("isDisappear", true);
        noteBoomVfx.SetActive(true);
    }

    // Instantiate miss feedback UI at the correct lane position.
    private void MissUI()
    {
        Vector3 spawnPos = Vector3.zero;

        // Get current scene index to determine which lane layout we're using
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Determine spawn position for 3-Rail mode (scene index 2)
        if (sceneIndex == 2)
        {
            if (column == 1)
                spawnPos = new Vector3(-198f, -458f, 0f);
            else if (column == 2)
                spawnPos = new Vector3(48f, -458f, 0f);
            else if (column == 3)
                spawnPos = new Vector3(287f, -458f, 0f);
        }
        // Determine spawn position for 4-Rail mode (scene index 3)
        else if (sceneIndex == 3)
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
        // Instantiate Miss UI prefab and position it at the calculated spawn position
        GameObject missUIInstance = Instantiate(missUIPrefab, missUIParent);
        missUIInstance.GetComponent<RectTransform>().localPosition = spawnPos;

        // Auto-destroy the miss UI after 1 second
        Destroy(missUIInstance, 1.0f);
    }
}
