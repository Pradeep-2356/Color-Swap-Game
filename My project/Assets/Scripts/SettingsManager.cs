using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Panels (assign these in Inspector)")]
    public GameObject settingsPanel;
    public GameObject pauseMenuUI;   // assign Pause menu panel GameObject
    public GameObject mainMenuUI;    // assign Main menu panel GameObject
    public GameObject gameOverUI;    // assign GameOver panel GameObject

    [Header("Sound Toggle Images (assign GameObjects)")]
    public GameObject soundOnImage;
    public GameObject soundOffImage;
    public GameObject musicOnImage;
    public GameObject musicOffImage;

    private bool isSoundOn = true;
    private bool isMusicOn = true;
    private string openedFrom = "Main";

    private void Awake()
    {
        // simple singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // load saved preferences
        isSoundOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        isMusicOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
    }

    private void Start()
    {
        // ensure panel hidden at start if assigned
        if (settingsPanel != null) settingsPanel.SetActive(false);

        UpdateSoundUI();
        UpdateMusicUI();
        ApplySoundSettings();
        // BGM state will be controlled via BGMManager if available
        if (BGMManager.Instance != null)
            BGMManager.Instance.ToggleBGM(isMusicOn);
    }

    // ----------------- OPEN / CLOSE -----------------
    public void OpenSettings(string source)
    {
        openedFrom = source;
        if (settingsPanel != null) settingsPanel.SetActive(true);

        // hide the source panel if assigned
        if (openedFrom == "Pause" && pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
        else if (openedFrom == "GameOver" && gameOverUI != null)
            gameOverUI.SetActive(false);
        else if (openedFrom == "Main" && mainMenuUI != null)
            mainMenuUI.SetActive(false);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);

        // restore the panel we opened from (if assigned)
        if (openedFrom == "Pause" && pauseMenuUI != null)
            pauseMenuUI.SetActive(true);
        else if (openedFrom == "GameOver" && gameOverUI != null)
            gameOverUI.SetActive(true);
        else if (openedFrom == "Main" && mainMenuUI != null)
            mainMenuUI.SetActive(true);
    }

    // ----------------- SOUND (SFX) -----------------
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundEnabled", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateSoundUI();
        ApplySoundSettings();
    }

    private void ApplySoundSettings()
    {
        // mute/unmute all objects tagged "SFX"
        var sfxObjects = GameObject.FindGameObjectsWithTag("SFX");
        foreach (var go in sfxObjects)
        {
            var a = go.GetComponent<AudioSource>();
            if (a != null) a.mute = !isSoundOn;
        }
    }

    private void UpdateSoundUI()
    {
        if (soundOnImage  != null) soundOnImage.SetActive(isSoundOn);
        if (soundOffImage != null) soundOffImage.SetActive(!isSoundOn);
    }

    // ----------------- MUSIC (BGM) -----------------
    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("MusicEnabled", isMusicOn ? 1 : 0);
        PlayerPrefs.Save();

        UpdateMusicUI();
        if (BGMManager.Instance != null)
            BGMManager.Instance.ToggleBGM(isMusicOn);
    }

    private void UpdateMusicUI()
    {
        if (musicOnImage  != null) musicOnImage.SetActive(isMusicOn);
        if (musicOffImage != null) musicOffImage.SetActive(!isMusicOn);
    }

    // convenience methods used by Pause / GameOver / etc.
    public void StopBGM()
    {
        if (BGMManager.Instance != null) BGMManager.Instance.StopBGM();
    }

    public void PlayBGM()
    {
        if (BGMManager.Instance != null && isMusicOn) BGMManager.Instance.PlayBGM();
    }
}
