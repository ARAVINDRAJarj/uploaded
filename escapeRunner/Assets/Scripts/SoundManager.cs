using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource coinSource;
    public AudioSource hitSource;
    public AudioSource bgMusic;

    void Awake()
    {
        // Singleton pattern to ensure one SoundManager persists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayCoin()
    {
        if (coinSource != null)
            coinSource.Play();
    }

    public void PlayHit()
    {
        if (hitSource != null)
            hitSource.Play();
    }

    public void PlayBackground()
    {
        if (bgMusic != null && !bgMusic.isPlaying)
            bgMusic.Play();
    }
}
