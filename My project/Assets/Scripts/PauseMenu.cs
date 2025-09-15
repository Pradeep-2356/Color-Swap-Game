using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu UI")]
    public GameObject pauseMenuUI;   // Pause panel
    public GameObject settingsMenuUI; // If you have separate settings inside pause, optional

    private bool isPaused = false;

    void Start()
    {
        // play BGM if manager present
        if (BGMManager.Instance != null)
            BGMManager.Instance.PlayBGM();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void OnPauseButton()
    {
        if (isPaused) Resume();
        else Pause();
    }

public void Resume()
{
    pauseMenuUI.SetActive(false);
    Time.timeScale = 1f;
    isPaused = false;

    SettingsManager.Instance.PlayBGM(); // resume BGM
}

public void Pause()
{
    pauseMenuUI.SetActive(true);
    Time.timeScale = 0f;
    isPaused = true;

    SettingsManager.Instance.StopBGM(); // stop BGM when paused
}

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // resume BGM after restart
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.PlayBGM();
        else if (BGMManager.Instance != null)
            BGMManager.Instance.PlayBGM();
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // Open settings using SettingsManager so back navigation goes to pause
    public void OpenSettings()
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.OpenSettings("Pause");
        else
        {
            // fallback: just show a local settings UI if you have one
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
            if (settingsMenuUI != null) settingsMenuUI.SetActive(true);
        }
    }

    public void BackToPauseMenu()
    {
        if (SettingsManager.Instance != null)
            SettingsManager.Instance.CloseSettings();
        else
        {
            if (settingsMenuUI != null) settingsMenuUI.SetActive(false);
            if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        }
    }
}
