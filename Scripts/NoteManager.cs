using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Represents one note entry in beatmap file.
[System.Serializable]
public class Note
{
    public string type;
    public int time;
    public int column;
}

// Represents full beatmap data containing both 3-rail and 4-rail notes.
[System.Serializable]
public class BeatmapData
{
    public List<Note> three_rail;
    public List<Note> four_rail;
}

/// NoteManager handles reading beatmap file, parsing note data,
/// and spawning notes at the correct time and lane.
public class NoteManager : MonoBehaviour
{
    // Beatmap file
    public string jsonFileName = "Thaehan - Doki-Doki (secXcscX) [Beginner]_converted.json";

    public GameObject notePrefab;

    // ---------- Note sprites for each lane ----------
    public Sprite noteSprite1;
    public Sprite noteSprite2;
    public Sprite noteSprite3;
    public Sprite noteSprite4;

    // ---------- Target transforms for each lane ----------
    public Transform lane1Target;
    public Transform lane2Target;
    public Transform lane3Target;
    public Transform lane4Target;

    // ---------- Spawner transforms for each lane ----------
    public Transform lane1Spawner;
    public Transform lane2Spawner;
    public Transform lane3Spawner;
    public Transform lane4Spawner;

    public float timingOffsetMs = 550f;

    // ---------- Internal variables ----------
    private List<Note> notes;
    private float songStartTime;
    private int spawnIndex = 0;
    private double songStartDspTime;

    // Read beatmap file and initialize note spawning system.
    void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Enable or disable 4th lane depending on scene
        if (sceneIndex == 3)
        {
            if (lane4Target != null)
                lane4Target.gameObject.SetActive(true);
            if (lane4Spawner != null)
                lane4Spawner.gameObject.SetActive(true);
        }
        else
        {
            if (lane4Target != null)
                lane4Target.gameObject.SetActive(false);
            if (lane4Spawner != null)
                lane4Spawner.gameObject.SetActive(false);
        }

        // Load beatmap file
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        if (!File.Exists(path))
        {
            return;
        }

        string json = File.ReadAllText(path);
        BeatmapData data = JsonUtility.FromJson<BeatmapData>(json);

        // Select notes depending on 3-rail or 4-rail mode
        if (sceneIndex == 3 && data.four_rail != null && data.four_rail.Count > 0)
            notes = data.four_rail;
        else if (data.three_rail != null && data.three_rail.Count > 0)
            notes = data.three_rail;
        else
            return;

        // Remove notes that would already be passed when song starts, spawn safety padding
        float minTravelDistance = Mathf.Abs(lane1Spawner.position.y - lane1Target.position.y);
        float speed = notePrefab.GetComponent<NoteMover>().speed;
        float travelTimeMs = minTravelDistance / speed * 1000f;
        float padding = timingOffsetMs;
        float earliestTime = 300f + travelTimeMs + padding;

        notes.RemoveAll(note => note.time < earliestTime);

        // Start playing music and initialize game state
        AudioManager.Instance.PlayMusic();
        songStartDspTime = AudioSettings.dspTime;
        GameManager.Instance.StartGame();
    }

    // Continuously checks whether to spawn the next note based on current music time.
    void Update()
    {
        if (GameManager.Instance.currentGameState != "Playing")
            return;

        if (notes == null || spawnIndex >= notes.Count)
            return;

        // Calculate current music time in high precision DSP clock
        float currentTimeMs = (float)((AudioSettings.dspTime - songStartDspTime) * 1000.0);

        Note nextNote = notes[spawnIndex];

        Transform spawner = GetSpawner(nextNote.column);
        Transform target = GetTarget(nextNote.column);

        // Calculate travel time for this specific note distance
        float actualYOffset = Mathf.Abs(spawner.position.y - target.position.y);
        float prefabSpeed = notePrefab.GetComponent<NoteMover>().speed;
        float travelTime = actualYOffset / prefabSpeed;
        float spawnTime = nextNote.time - travelTime * 1000f - timingOffsetMs;

        // Spawn note when it's time
        if (currentTimeMs >= spawnTime)
        {
            GameObject note = Instantiate(notePrefab, spawner.position, Quaternion.identity);

            // Assign correct sprite based on column
            SpriteRenderer sr = note.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                switch (nextNote.column)
                {
                    case 1:
                        sr.sprite = noteSprite1;
                        break;
                    case 2:
                        sr.sprite = noteSprite2;
                        break;
                    case 3:
                        sr.sprite = noteSprite3;
                        break;
                }
            }

            // Pass target timing info to NoteHandler
            var handler = note.GetComponent<NoteHandler>();
            double absoluteTargetTimeMs = (songStartDspTime * 1000.0) + nextNote.time;
            handler.column = nextNote.column;
            handler.targetTimeMs = absoluteTargetTimeMs;

            spawnIndex++;
        }
    }

    // Helper function to get correct spawner transform based on column.
    Transform GetSpawner(int col)
    {
        return col switch
        {
            1 => lane1Spawner,
            2 => lane2Spawner,
            3 => lane3Spawner,
            4 => lane4Spawner,
            _ => null,
        };
    }

    // Helper function to get correct target transform based on column.
    Transform GetTarget(int col)
    {
        return col switch
        {
            1 => lane1Target,
            2 => lane2Target,
            3 => lane3Target,
            4 => lane4Target,
            _ => null,
        };
    }
}
