using UnityEngine;
using System.Collections.Generic;

public class EnvironmentSpawner : MonoBehaviour
{
    public GameObject environmentPrefab;
    public Transform player;
    public int initialEnvs = 3;

    private float envLength;
    private float spawnZ = 0f;
    private Queue<GameObject> activeEnvs = new Queue<GameObject>();

    void Start()
    {
        // Measure prefab length automatically
        Renderer prefabRenderer = environmentPrefab.GetComponentInChildren<Renderer>();
        envLength = prefabRenderer.bounds.size.z;

        for (int i = 0; i < initialEnvs; i++)
        {
            SpawnEnv();
        }
    }

    void Update()
    {
        if (player.position.z - 2 * envLength > (spawnZ - initialEnvs * envLength))
        {
            SpawnEnv();
            DeleteEnv();
        }
    }

    void SpawnEnv()
    {
        GameObject go = Instantiate(environmentPrefab, Vector3.forward * spawnZ, Quaternion.identity);
        activeEnvs.Enqueue(go);
        spawnZ += envLength; // no gaps
    }

    void DeleteEnv()
    {
        GameObject oldEnv = activeEnvs.Dequeue();
        Destroy(oldEnv);
    }
}
