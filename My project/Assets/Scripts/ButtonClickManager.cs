using UnityEngine;

public class ButtonClickManager : MonoBehaviour
{
    public static ButtonClickManager Instance;

    [Header("Click Sound")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persist across scenes if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
