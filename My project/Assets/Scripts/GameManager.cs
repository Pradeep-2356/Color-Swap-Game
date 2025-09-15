using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

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

    [Header("Player Reference")]
    public PlayerController player;             

    [Header("Settings Panel")]
    public GameObject settingsPanel;  

    [Header("Audio")]
    public AudioSource gameOverAudio; // Assign the Game Over audio clip in Inspector

    private float currentScore = 0f;  
    private int highScore = 0;
    private int scoreMultiplier = 1;
    private Coroutine multiplierRoutine;

void Start()
{
    isGameOver = false;
    Time.timeScale = 1f;
    gameOverUI.SetActive(false);

    if (settingsPanel != null)
        settingsPanel.SetActive(false);

    if (gameOverAudio != null)
        gameOverAudio.Stop(); // ensure it doesn't play at start

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

        currentScore += player.forwardSpeed * Time.deltaTime * scoreMultiplier;
        UpdateUI();
    }

    void UpdateUI()
    {
        int displayScore = Mathf.FloorToInt(currentScore);

        if (scoreValueText != null)
            scoreValueText.text = "Score: " + displayScore.ToString();

        if (liveScoreText != null)
            liveScoreText.text = "Score: " + displayScore.ToString();

        if (highScoreValueText != null)
            highScoreValueText.text = "High Score: " + highScore.ToString();
    }

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
        SettingsManager.Instance.StopBGM(); // stop BGM on Game Over

        // Play Game Over sound
        if (gameOverAudio != null && !gameOverAudio.isPlaying)
            gameOverAudio.Play();

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
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            Debug.Log("Settings panel opened!");
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            Debug.Log("Settings panel closed!");
        }
    }
}
