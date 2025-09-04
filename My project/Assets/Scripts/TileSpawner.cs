using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [Header("Tile Settings")]
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

    [Header("Props Settings")]
    public GameObject[] propPrefabs;                     // obstacles + powerups
    [Range(0f, 1f)] public float propSpawnChance = 0.35f;
    public Vector3 propOffset = new Vector3(0f, 0.6f, 0f);
    public float minPropSpacing = 4f;                    // in Z units
    private float lastPropZ = -999f;

    [Header("For Gulal")]
    private bool forceSingleColor = false;
    private ColorType forcedColor;
    private float forceColorTimer = 0f;

    void Start()
    {
        spawnZ = player.position.z;
        for (int i = 0; i < initialTiles; i++) SpawnTile();
    }

    void Update()
    {
        if (GameManager.isGameOver) return;

        // countdown forced mode
        if (forceSingleColor && forceColorTimer > 0f)
        {
            forceColorTimer -= Time.deltaTime;
            if (forceColorTimer <= 0f)
            {
                forceSingleColor = false;
                Debug.Log("Gulal effect ended, back to normal tiles.");
            }
        }

        while (spawnZ - player.position.z < lookAheadTiles * tileLength)
            SpawnTile();
    }

    void SpawnTile()
    {
        Vector3 spawnPos = new Vector3(0, 0, spawnZ);
        GameObject go = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
        go.transform.SetParent(transform);

        Renderer rend = go.GetComponent<Renderer>();
        Tile tileScript = go.AddComponent<Tile>();

        ColorType ct = GetNextColorType();
        tileScript.tileColorType = ct;

        if (rend != null)
            rend.material.color = ColorFromType(ct);

        TrySpawnProp(go.transform);   // <-- props here

        spawnZ += tileLength;
        tilesSpawned++;

        if (tilesSpawned % 40 == 0)
        {
            currentLevel++;
            Debug.Log("Level Up! Now Level " + currentLevel);
        }
    }

    void TrySpawnProp(Transform tileTransform)
    {
        if (propPrefabs == null || propPrefabs.Length == 0) return;
        if (Random.value >= propSpawnChance) return;
        if (spawnZ - lastPropZ < minPropSpacing) return;

        GameObject prefab = propPrefabs[Random.Range(0, propPrefabs.Length)];

        float randomX = Random.Range(-0.7f, 0.7f);
        Vector3 worldPos = tileTransform.position + propOffset + new Vector3(randomX, 0, 0);

        // Instantiate without modifying scale or rotation yet
        GameObject prop = Instantiate(prefab, worldPos, prefab.transform.rotation);

        // Detach first to avoid scale inheritance issues
        prop.transform.SetParent(null);

        // Restore prefab's original scale and rotation (defensive)
        prop.transform.localScale = prefab.transform.localScale;
        prop.transform.rotation = prefab.transform.rotation;

        // Parent to tile after fixing scale/rotation
        prop.transform.SetParent(tileTransform, true);

        // Fix particle systems
        var sourceChildren = prefab.GetComponentsInChildren<Transform>(true);
        var spawnedParticles = prop.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in spawnedParticles)
        {
            var main = ps.main;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;

            Transform srcMatch = FindDeepChildByName(sourceChildren, ps.transform.name);
            if (srcMatch != null)
            {
                ps.transform.localRotation = srcMatch.localRotation;
            }
        }

        lastPropZ = spawnZ;
    }


    // helper: find a matching child Transform by name in the prefab's children list (returns first match)
    Transform FindDeepChildByName(Transform[] list, string name)
    {
        foreach (var t in list)
        {
            if (t.name == name) return t;
        }
        return null;
    }

    ColorType GetNextColorType()
    {
        if (forceSingleColor)
            return forcedColor;

        if (tilesSpawned < 5) return ColorType.Red;

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
            default: return Color.white;
        }
    }

    public void ActivateGulal(ColorType color, float duration)
    {
        forcedColor = color;
        forceSingleColor = true;
        forceColorTimer = duration;

        // recolor all existing tiles
        Tile[] allTiles = FindObjectsOfType<Tile>();
        foreach (Tile t in allTiles)
        {
            t.SetColor(color);
        }

        Debug.Log("Gulal activated! Tiles are all " + color + " for " + duration + "s");
    }

}
