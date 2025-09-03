using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void ResumeGame()
    {
        GameManager.Instance.TogglePause();
    }

    public void ResetGame()
    {
        GameManager.Instance.ResetGame();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
