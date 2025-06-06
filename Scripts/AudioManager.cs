using UnityEngine;

/// Singleton AudioManager to control background music across scenes.
/// Handles music play, pause, resume, and provides time data for syncing.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;
    public AudioClip defaultMusic; // Default music clip to play

    // Initialize singleton instance and AudioSource component.
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.playOnAwake = false;
            }

            // Assign default music if provided
            if (defaultMusic != null)
            {
                musicSource.clip = defaultMusic;
                musicSource.playOnAwake = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play musicSource if not already playing.
    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    // Pause the music if currently playing.
    public void PauseMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
    }

    // Resume music playback.
    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    // Get current music playback time in milliseconds.
    public float GetMusicTimeInMs()
    {
        return musicSource.time * 1000f;
    }

    // Get current DSP time in milliseconds.
    public double GetDspTimeInMs()
    {
        return AudioSettings.dspTime * 1000.0;
    }
}
