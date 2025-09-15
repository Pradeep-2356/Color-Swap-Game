using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [Header("Background Music")]
    public AudioSource bgmSource;   // assign AudioSource with your music clip

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // keep playing across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
            bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
            bgmSource.Stop();
    }

    public void ToggleBGM(bool enabled)
    {
        if (bgmSource != null)
        {
            bgmSource.mute = !enabled;
        }
    }
}
