using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public int initialTiles = 10;
    public float tileLength = 2f;
    public Transform player;

    private float spawnZ;
    private int tilesSpawned = 0;
    private int currentLevel = 1;

    private ColorType currentStreakColor;
    private int tilesLeftInStreak = 0;

    public int lookAheadTiles = 5;

    void Start()
    {
        spawnZ = player.position.z;
        for (int i = 0; i < initialTiles; i++) SpawnTile();
    }

void Update()
{
    if (GameManager.isGameOver) return;

    while (spawnZ - player.position.z < lookAheadTiles * tileLength)
    {
        SpawnTile();
    }
}


    void SpawnTile()
    {
        Vector3 spawnPos = new Vector3(0, 0, spawnZ);
        GameObject go = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
        go.transform.SetParent(transform);

        Renderer rend = go.GetComponent<Renderer>();
        Tile tileScript = go.AddComponent<Tile>(); // give tile logic

        ColorType ct = GetNextColorType();
        tileScript.tileColorType = ct;

        if (rend != null)
        {
            rend.material.color = ColorFromType(ct);
        }

        spawnZ += tileLength;
        tilesSpawned++;

        if (tilesSpawned % 40 == 0)
        {
            currentLevel++;
            Debug.Log("Level Up! Now Level " + currentLevel);
        }
    }

    ColorType GetNextColorType()
    {
            // First 5 tiles (or however many you want) should be red
        if (tilesSpawned < 5)
        {
        return ColorType.Red;
        }
        
        if (tilesLeftInStreak > 0)
        {
            tilesLeftInStreak--;
            return currentStreakColor;
        }

        int streakLength = GetStreakLengthForLevel(currentLevel);
        currentStreakColor = (ColorType)Random.Range(0, 4);
        tilesLeftInStreak = streakLength - 1;

        return currentStreakColor;
    }

    int GetStreakLengthForLevel(int level)
    {
        if (level == 1) return Random.Range(5, 7);
        if (level <= 3) return Random.Range(4, 6);
        if (level <= 5) return Random.Range(3, 5);
        if (level <= 8) return Random.Range(2, 4);
        return Random.Range(1, 3);
    }

    Color ColorFromType(ColorType ct)
    {
        switch (ct)
        {
            case ColorType.Red: return Color.red;
            case ColorType.Blue: return Color.blue;
            case ColorType.Green: return Color.green;
            case ColorType.Yellow: return Color.yellow;
        }
        return Color.white;
    }
}
