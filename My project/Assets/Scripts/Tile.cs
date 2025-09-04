using UnityEngine;

public class Tile : MonoBehaviour
{
    public ColorType tileColorType;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    public void SetColor(ColorType newColor)
    {
        tileColorType = newColor;
        if (rend != null)
        {
            rend.material.color = ColorFromType(newColor);
        }
    }

    Color ColorFromType(ColorType ct)
    {
        switch (ct)
        {
            case ColorType.Red: return Color.red;
            case ColorType.Blue: return Color.blue;
            case ColorType.Green: return Color.green;
            case ColorType.Yellow: return Color.yellow;
            default: return Color.white;
        }
    }
}
