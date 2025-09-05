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
        int score = PlayerPrefs.GetInt("Highscore", 0);
        highscoreText.text = "" + score;

        highscorePanel.SetActive(true);
        highscoreAnimator.SetTrigger("Show");
    }

    public void HideHighscore()
    {
        highscoreAnimator.SetTrigger("Hide");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit pressed");
    }
}
