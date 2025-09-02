using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public int initialTiles = 10;
    public float tileLength = 2f;
    public Transform player;

    private float spawnZ;
    private int safeZone = 15;
    private int tilesOnScreen = 10;

    private int currentLevel = 1;
    private int tilesSpawned = 0;

    private Color currentStreakColor;
    private int tilesLeftInStreak = 0;
public int lookAheadTiles = 5; // Number of tiles ahead player can see

    void Start()
    {
        spawnZ = player.position.z;

        for (int i = 0; i < initialTiles; i++)
        {
            SpawnTile();
        }
    }


void Update()
{
    // Spawn tiles until there are enough ahead of player
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

        // Assign color
        Renderer rend = go.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = GetNextColor();
        }

        spawnZ += tileLength;
        tilesSpawned++;

        // Level up every 40 tiles (slower progression)
        if (tilesSpawned % 40 == 0)
        {
            currentLevel++;
            Debug.Log("Level Up! Now Level " + currentLevel);
        }
    }

    Color GetNextColor()
    {
        if (tilesLeftInStreak > 0)
        {
            tilesLeftInStreak--;
            return currentStreakColor;
        }

        // Decide streak length based on current level (gradual change)
        int streakLength = GetStreakLengthForLevel(currentLevel);

        currentStreakColor = GetRandomColor();
        tilesLeftInStreak = streakLength - 1;

        return currentStreakColor;
    }

    int GetStreakLengthForLevel(int level)
    {
        if (level == 1) return Random.Range(5, 7);   // very easy, long streak
        if (level <= 3) return Random.Range(4, 6);   // still easy
        if (level <= 5) return Random.Range(3, 5);   // moderate
        if (level <= 8) return Random.Range(2, 4);   // harder
        return Random.Range(1, 3);                   // very challenging
    }

    Color GetRandomColor()
    {
        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0: return Color.red;
            case 1: return Color.green;
            case 2: return Color.blue;
            default: return Color.yellow;
        }
    }
}
