using UnityEngine;
using UnityEngine.UI;

public class ColorSelectorUI : MonoBehaviour
{
    public Image[] colorBoxes;         // Assign the Red, Green, Blue, Yellow boxes
    public RectTransform selectionBorder;  // Assign the black border

    public PlayerController player;    // Reference to your player

    void Start()
    {
        UpdateSelectionBorder();       // Set initial position
    }

    void Update()
    {
        // Keep updating border if player changes color
        UpdateSelectionBorder();
    }

    void UpdateSelectionBorder()
    {
        int colorIndex = (int)player.currentColor;

        if(colorIndex >= 0 && colorIndex < colorBoxes.Length)
        {
            // Move the border to selected box
            selectionBorder.position = colorBoxes[colorIndex].transform.position;
        }
    }
}
