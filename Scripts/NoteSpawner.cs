using System.Collections;
using UnityEngine;

// (Debug Only) Simple test spawner to repeatedly generate notes at fixed intervals.
// Used during early testing phase before beatmap system was implemented.
public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab; // Prefab to spawn
    public float spawnInterval = 2f; // time between each spawn

    void Start()
    {
        if (notePrefab == null)
        {
            return;
        }
        StartCoroutine(SpawnNotes());
    }

    IEnumerator SpawnNotes()
    {
        while (true)
        {
            Instantiate(notePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
