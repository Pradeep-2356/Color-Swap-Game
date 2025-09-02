using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver = false;

    public static void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("GAME OVER!");
        }
    }
}
