using UnityEngine;
using System.Collections;

// MusicPlayer handles background music playback with optional countdown delay before starting.
// Implements singleton pattern for global access.
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    // ---------- Internal state ----------
    private AudioSource audioSource;
    private float musicStartTime;
    private bool isMusicPlaying = false;
    private bool hasStartedOnce = false;

    // Public property for elapsed music time
    public int MusicElapsedMilliseconds => isMusicPlaying ? (int)((Time.time - musicStartTime) * 1000) : 0;

    // Initialize singleton instance and audio source.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            // Load music file from Resources folder
            audioSource.clip = Resources.Load<AudioClip>("music/kickback");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play music with countdown (only first time), or resume if already started once.
    public void PlayMusicWithCountdown()
    {
        if (!isMusicPlaying && audioSource.clip != null)
        {
            if (!hasStartedOnce)
            {
                StartCoroutine(CountdownAndPlay());
            }
            else
            {
                ResumeMusic();
            }
        }
    }

    // Coroutine for 3-second countdown before playing music.
    private IEnumerator CountdownAndPlay()
    {
        yield return new WaitForSeconds(3f);

        audioSource.Play();
        musicStartTime = Time.time;
        isMusicPlaying = true;
        hasStartedOnce = true;
    }

    // Stop music and reset state.
    public void StopMusic()
    {
        audioSource.Stop();
        isMusicPlaying = false;
    }

    // Check if music is playing.
    public bool IsMusicPlaying()
    {
        return isMusicPlaying;
    }

    // Pause music playback.
    public void PauseMusic()
    {
        if (isMusicPlaying && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }
    
    // Resume music playback.
    public void ResumeMusic()
    {
        if (isMusicPlaying && !audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
}
