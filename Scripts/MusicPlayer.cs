using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;

    private AudioSource audioSource;
    private float musicStartTime;
    private bool isMusicPlaying = false;

    public int MusicElapsedMilliseconds => isMusicPlaying ? (int)((Time.time - musicStartTime) * 1000) : 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = Resources.Load<AudioClip>("music/kickback");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusicWithCountdown()
    {
        if (!isMusicPlaying && audioSource.clip != null)
        {
            StartCoroutine(CountdownAndPlay());
        }
    }

    private IEnumerator CountdownAndPlay()
    {
        yield return new WaitForSeconds(3f);

        audioSource.Play();
        musicStartTime = Time.time;
        isMusicPlaying = true;
    }

    public void StopMusic()
    {
        audioSource.Stop();
        isMusicPlaying = false;
    }

    public bool IsMusicPlaying()
    {
        return isMusicPlaying;
    }

    public void PauseMusic()
    {
        if (isMusicPlaying && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (isMusicPlaying && !audioSource.isPlaying)
        {
            audioSource.UnPause(); 
        }
    }

}

