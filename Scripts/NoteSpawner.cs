using System.Collections;
using UnityEngine;

// Repeatedly spawn a certain notePrefab.
public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;   // Prefab to spawn
    public float spawnInterval = 2f;  // time between each spawn

    void Start()
    {
        if (notePrefab == null)
        {
            //Debug.LogError("NotePrefab is not assigned.");
            return;
        }
        StartCoroutine(SpawnNotes());
    }

    IEnumerator SpawnNotes()
    {
        while (true)
        {
            Instantiate(notePrefab, transform.position, Quaternion.identity);
            //Debug.Log("Note spawned at: " + transform.position);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
