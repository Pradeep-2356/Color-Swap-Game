using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Renderer playerRenderer;
    public Color[] colors;  // Red, Green, Blue, Yellow
    private int currentColorIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeColor(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeColor(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeColor(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeColor(3);
    }

    void ChangeColor(int index)
    {
        currentColorIndex = index;
        playerRenderer.material.color = colors[index];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            Tile tile = other.GetComponent<Tile>();
            if (tile.tileColorIndex == currentColorIndex)
            {
                GameManager.Instance.UpdateScore(10);
            }
            else
            {
                Debug.Log("Game Over!");
                GameManager.Instance.TogglePause(); // freeze on fail
            }
        }
    }
}
