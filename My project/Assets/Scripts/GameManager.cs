using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false;

    [Header("Game Over UI")]
    public GameObject gameOverUI;
    public TextMeshProUGUI scoreValueText;
    public TextMeshProUGUI highScoreValueText;
    public TextMeshProUGUI newHighScoreText;

    [Header("Live Score UI")]
    public TextMeshProUGUI liveScoreText;


    private int currentScore = 0;
    private int highScore = 0;

    void Start()
    {
        isGameOver = false;
        gameOverUI.SetActive(false);

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreValueText.text = highScore.ToString();
        newHighScoreText.gameObject.SetActive(false);
    }

    public void UpdateScore(int amount)
    {
        currentScore = amount;

        // For Game Over panel
        scoreValueText.text = "Score: " + currentScore.ToString();

        // For live score top-left
        if (liveScoreText != null)
        liveScoreText.text = "Score: " + currentScore.ToString();
    }


    public static void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            FindObjectOfType<GameManager>().ShowGameOver();
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        scoreValueText.text = "Score: " + currentScore.ToString();
    }

    void ShowGameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);

        scoreValueText.text = "Score: " + currentScore.ToString();
        highScoreValueText.text = "High Score: " + highScore.ToString();


        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            highScoreValueText.text = "High Score: " + highScore.ToString();
            newHighScoreText.gameObject.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettings()
    {
        Debug.Log("Open settings panel here.");
    }


}
