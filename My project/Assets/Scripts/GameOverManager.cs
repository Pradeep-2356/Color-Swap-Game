using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverUI;        
    public TextMeshProUGUI scoreValueText;     // only number after "SCORE:"
    public TextMeshProUGUI highScoreValueText; // only number after "HIGH SCORE:"
    public TextMeshProUGUI newHighScoreText;   // "NEW HIGH SCORE!" text

    private int currentScore = 0;
    private int highScore = 0;

    void Start()
    {
        gameOverUI.SetActive(false);

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Hide "NEW HIGH SCORE!" initially
        newHighScoreText.gameObject.SetActive(false);

        // Initialize values (blank or 0)
        scoreValueText.text = "0";
        highScoreValueText.text = highScore.ToString();
    }

    public void GameOver(int finalScore)
    {
        currentScore = finalScore;

        gameOverUI.SetActive(true);
        Time.timeScale = 0f;

        // update only the number parts
        scoreValueText.text = currentScore.ToString();
        highScoreValueText.text = highScore.ToString();

        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            highScoreValueText.text = highScore.ToString();
            newHighScoreText.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenSettings()
    {
        Debug.Log("Open Settings Panel here");
    }

    public void GoToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
