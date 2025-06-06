using UnityEngine;

// Handles downward movement of note objects at constant speed.
// Each instantiated note uses this to move toward target.
public class NoteMover : MonoBehaviour
{
    public float speed = 5f;

    /// Called once when the note is created for debugging.
    void Start()
    {
        Debug.Log("Note speed is: " + speed);
    }

    // Move the note downward every frame based on speed.
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
