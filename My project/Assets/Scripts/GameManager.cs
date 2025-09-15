using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false;

    [Header("Game Over UI")]
    public GameObject gameOverUI;
    public TextMeshProUGUI scoreValueText;      // final score
    public TextMeshProUGUI highScoreValueText;  // final high score
    public TextMeshProUGUI newHighScoreText;    // "NEW HIGH SCORE!" badge

    [Header("Live Score UI")]
    public TextMeshProUGUI liveScoreText;       // top-left live score

    [Header("Player Reference")]
    public PlayerController player;             // assign Player in Inspector

    private float currentScore = 0f;  // float for smoother increment
    private int highScore = 0;

    // ⭐ Star multiplier
    private int scoreMultiplier = 1;
    private Coroutine multiplierRoutine;

    void Start()
    {
        isGameOver = false;
        Time.timeScale = 1f;
        gameOverUI.SetActive(false);

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScoreValueText != null)
            highScoreValueText.text = "High Score: " + highScore.ToString();

        if (newHighScoreText != null)
            newHighScoreText.gameObject.SetActive(false);

        currentScore = 0;
        UpdateUI();
    }

    void Update()
    {
        if (isGameOver || player == null) return;

        // 🏃 Score grows with speed & time
        currentScore += player.forwardSpeed * Time.deltaTime * scoreMultiplier;

        UpdateUI();
    }

    void UpdateUI()
    {
        int displayScore = Mathf.FloorToInt(currentScore);

        // Final score
        if (scoreValueText != null)
            scoreValueText.text = "Score: " + displayScore.ToString();

        // Live score
        if (liveScoreText != null)
            liveScoreText.text = "Score: " + displayScore.ToString();

        // High score
        if (highScoreValueText != null)
            highScoreValueText.text = "High Score: " + highScore.ToString();
    }

    // ⭐ Called when player collects Star
    public void ActivateDoubleScore(float duration)
    {
        if (multiplierRoutine != null)
            StopCoroutine(multiplierRoutine);

        multiplierRoutine = StartCoroutine(DoubleScoreRoutine(duration));
    }

    private IEnumerator DoubleScoreRoutine(float duration)
    {
        scoreMultiplier = 2;
        yield return new WaitForSeconds(duration);
        scoreMultiplier = 1;
    }

    public static void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            FindObjectOfType<GameManager>().ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);

        int finalScore = Mathf.FloorToInt(currentScore);

        if (scoreValueText != null)
            scoreValueText.text = "Score: " + finalScore.ToString();

        if (highScoreValueText != null)
            highScoreValueText.text = "High Score: " + highScore.ToString();

        if (finalScore > highScore)
        {
            highScore = finalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();

            if (highScoreValueText != null)
                highScoreValueText.text = "High Score: " + highScore.ToString();

            if (newHighScoreText != null)
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
