using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Note
{
    public string type;
    public int time;
    public int column;
}

[System.Serializable]
public class BeatmapData
{
    public List<Note> three_rail;
    public List<Note> four_rail;
}

public class NoteManager : MonoBehaviour
{
    public string jsonFileName = "Thaehan - Doki-Doki (secXcscX) [Beginner]_converted.json";
    //public AudioSource musicAudioSource;
    public GameObject notePrefab;

    public Sprite noteSprite1;
    public Sprite noteSprite2;
    public Sprite noteSprite3;
    public Sprite noteSprite4;


    public Transform lane1Target;
    public Transform lane2Target;
    public Transform lane3Target;
    public Transform lane4Target;


    public Transform lane1Spawner;
    public Transform lane2Spawner;
    public Transform lane3Spawner;
    public Transform lane4Spawner;



    public float timingOffsetMs = 550f;

    //public float noteSpeed = 5f;
    //public float spawnYOffset = 10f;

    private List<Note> notes;
    private float songStartTime;
    private int spawnIndex = 0;
    private double songStartDspTime;

    void Start()
    {

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 3)
        {
            if (lane4Target != null) lane4Target.gameObject.SetActive(true);
            if (lane4Spawner != null) lane4Spawner.gameObject.SetActive(true);
        }
        else
        {
            if (lane4Target != null) lane4Target.gameObject.SetActive(false);
            if (lane4Spawner != null) lane4Spawner.gameObject.SetActive(false);
        }

        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (!File.Exists(path))
        {
            return;
        }

        string json = File.ReadAllText(path);
        BeatmapData data = JsonUtility.FromJson<BeatmapData>(json);

        // if (data == null || data.three_rail == null || data.three_rail.Count == 0)
        // {
        //     //Debug.LogError("Failed to parse JSON or no notes found.");
        //     return;
        // }

        // notes = data.three_rail;

        // ---------depend rail 3/4----------
        if (sceneIndex == 3 && data.four_rail != null && data.four_rail.Count > 0)
            notes = data.four_rail;
        else if (data.three_rail != null && data.three_rail.Count > 0)
            notes = data.three_rail;
        else
            return;
        // -----------------------------------------------
            
        // ---------- to avoid duplicated notes when music starts ---------- 
        float minTravelDistance = Mathf.Abs(lane1Spawner.position.y - lane1Target.position.y); 
        float speed = notePrefab.GetComponent<NoteMover>().speed;
        float travelTimeMs = minTravelDistance / speed * 1000f;
        float padding = timingOffsetMs;
        float earliestTime = 300f + travelTimeMs + padding;

        notes.RemoveAll(note => note.time < earliestTime);
        // -----------------------------------------------------------------
   
        //musicAudioSource.Play();
        //songStartTime = Time.time; 
        //songStartTime = (float)AudioSettings.dspTime;
        //GameManager.Instance.StartGame();

        AudioManager.Instance.PlayMusic();
        songStartDspTime = AudioSettings.dspTime;
        GameManager.Instance.StartGame();
    }


    void Update()
    {
        if (GameManager.Instance.currentGameState != "Playing")
            return;

        if (notes == null || spawnIndex >= notes.Count)
            return;

        //float currentTimeMs = (Time.time - songStartTime) * 1000f;
        //float currentTimeMs = (float)((AudioSettings.dspTime - songStartTime) * 1000.0);
        float currentTimeMs = (float)((AudioSettings.dspTime - songStartDspTime) * 1000.0);


        Note nextNote = notes[spawnIndex];

        Transform spawner = GetSpawner(nextNote.column);
        Transform target = GetTarget(nextNote.column); 
       
        float actualYOffset = Mathf.Abs(spawner.position.y - target.position.y);
        float prefabSpeed = notePrefab.GetComponent<NoteMover>().speed;
        
        float travelTime = actualYOffset / prefabSpeed;
        float spawnTime = nextNote.time - travelTime * 1000f - timingOffsetMs;

        if (currentTimeMs >= spawnTime)
        {
            GameObject note = Instantiate(notePrefab, spawner.position, Quaternion.identity);
            
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
            var handler = note.GetComponent<NoteHandler>();
            // var handler = note.GetComponent<NoteHandler>();
            // handler.targetTimeMs = nextNote.time;
            //handler.musicSource = musicAudioSource;
            double absoluteTargetTimeMs = (songStartDspTime * 1000.0) + nextNote.time;
            handler.column = nextNote.column; 
            handler.targetTimeMs = absoluteTargetTimeMs;

            spawnIndex++;
        }
    }
    Transform GetSpawner(int col)
    {
        return col switch
        {
            1 => lane1Spawner,
            2 => lane2Spawner,
            3 => lane3Spawner,
            4 => lane4Spawner,
            _ => null
        };
    }
    Transform GetTarget(int col)
    {
        return col switch
        {
            1 => lane1Target,
            2 => lane2Target,
            3 => lane3Target,
            4 => lane4Target,
            _ => null
        };
    }


    // Transform GetSpawner(int col)
    // {
    //     return col switch
    //     {
    //         1 => lane1Spawner,
    //         2 => lane2Spawner,
    //         3 => lane3Spawner,
    //         _ => null
    //     };
    // }
    // Transform GetTarget(int col)
    // {
    //     return col switch
    //     {
    //         1 => lane1Target,
    //         2 => lane2Target,
    //         3 => lane3Target,
    //         _ => null
    //     };
    // }
}
