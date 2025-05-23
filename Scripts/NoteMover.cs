using UnityEngine;

public class NoteMover : MonoBehaviour
{
    public float speed = 5f;
    void Start()
    {
    Debug.Log("Note speed is: " + speed);
    }


    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}