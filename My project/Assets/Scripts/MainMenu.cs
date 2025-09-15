using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Music Toggle")]
    public GameObject musicOnIcon;   // assign image for "music enabled"
    public GameObject musicOffIcon;  // assign image for "music disabled"
    private bool isMuted = false;

    [Header("Highscore Panel")]
    public GameObject highscorePanel;
    public Animator highscoreAnimator;
    public TMP_Text highscoreText;

    void Start()
    {
        // Initialize music state
        isMuted = AudioListener.pause;
        musicOnIcon.SetActive(!isMuted);
        musicOffIcon.SetActive(isMuted);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;
        AudioListener.pause = isMuted;

        musicOnIcon.SetActive(!isMuted);
        musicOffIcon.SetActive(isMuted);
    }

    public void ShowHighscore()
    {
        int score = PlayerPrefs.GetInt("HighScore", -1); // match GameManager key

        if (score > 0)
            highscoreText.text = score.ToString();
        else
            highscoreText.text = "No Highscore Yet";

        highscorePanel.SetActive(true);
        if (highscoreAnimator != null)
            highscoreAnimator.SetTrigger("Show");
    }

    public void HideHighscore()
    {
        if (highscoreAnimator != null)
            highscoreAnimator.SetTrigger("Hide");
        else
            highscorePanel.SetActive(false);
    }

    public void OpenSettings()
    {
        Debug.Log("Open settings panel here.");
        // TODO: enable your settings UI when ready
    }
}
