using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static TerrainSpawnMobManager.SectionSpawnParams;

public class TerrainSpawnMobManager : MonoBehaviour
{
    [System.Serializable]
    public class SectionSpawnParams
    {
        [System.Serializable]
        public class SpawnPrefabInfo
        {
            public GameObject prefab; // The prefab to spawn
            public int spawnCount;    // The number of objects to spawn for this prefab in this section
            public int currentSpawnedCount; // The number of objects spawned for this prefab in this section
        }

        public List<SpawnPrefabInfo> spawnPrefabs;       // List of game object prefabs and their spawn counts
        public GameObject sectionGameObject;            // GameObject representing the section
        public List<GameObject> spawnedObjects;         // List to store spawned objects in this section
    }

    public SectionSpawnParams[] sectionSpawnParams;
    public Transform player;
    public LayerMask groundMask;
    public float spawnInnerRadius = 10f;
    public float spawnOuterRadius = 20f;
    public float despawnRadius = 30f;
    public float spawnInterval = 5f;
    public int maxSpawnCount = 50;

    private int totalSpawnedObjects = 0;
    private float spawnTimer = 0f;

    void Update()
    {
        // Spawning logic
        if(totalSpawnedObjects >= maxSpawnCount)
        {
            // Reset the spawn timer
            spawnTimer = spawnInterval;
        }
        if (spawnTimer <= 0f && totalSpawnedObjects < maxSpawnCount)
        {
            // Get a random spawn point around the player
            Vector3 spawnPoint = GetRandomSpawnPoint();

            // Spawn objects
            SpawnObjectsAtPoint(spawnPoint);

        }
        else
        {
            // Update spawn timer
            spawnTimer -= Time.deltaTime;
        }

        // Despawning logic
        DespawnObjectsOutsideRadius();
    }

    Vector3 GetRandomSpawnPoint()
    {
        Vector3 playerPosition = player.position;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float distance = Random.Range(spawnInnerRadius, spawnOuterRadius);
        Vector3 spawnPoint = playerPosition + new Vector3(randomDirection.x, 0f, randomDirection.y) * distance;

        return spawnPoint;
    }

    void SpawnObjectsAtPoint(Vector3 spawnPoint)
    {
        // Find the section where the spawn point is located
        SectionSpawnParams currentSection = GetCurrentSection(spawnPoint);

        if (currentSection == null)
        {
            Debug.LogWarning("No section found for spawn point: " + spawnPoint);
            return;
        }

        // Shuffle spawn prefabs list to randomize selection
        Shuffle(currentSection.spawnPrefabs);

        foreach (SpawnPrefabInfo prefabInfo in currentSection.spawnPrefabs)
        {
            if (prefabInfo.currentSpawnedCount < prefabInfo.spawnCount && totalSpawnedObjects < maxSpawnCount)
            {
                // Attempt to spawn around the spawn point
                Vector3 randomOffset = Random.insideUnitSphere * spawnOuterRadius;
                randomOffset.y = 0f; // Keep the y-offset zero initially

                // Cast a ray from the spawn point downwards to find the terrain hit point
                RaycastHit hit;
                if (Physics.Raycast(spawnPoint + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, groundMask))
                {
                    // Adjust spawn position to the terrain hit point
                    Vector3 spawnPosition = hit.point + randomOffset;

                    // Instantiate the prefab at the adjusted spawn position
                    GameObject obj = Instantiate(prefabInfo.prefab, spawnPosition, Quaternion.identity);
                    obj.GetComponent<MobBlackboard>().Target = player.gameObject;
                    currentSection.spawnedObjects.Add(obj);

                    // Increment the spawn count for this prefab
                    prefabInfo.currentSpawnedCount++;
                    totalSpawnedObjects++;
                }
                else
                {
                    Debug.LogWarning("No terrain found below spawn point: " + spawnPoint);
                }
            }
        }
    }

    void DespawnObjectsOutsideRadius()
    {
        foreach (SectionSpawnParams sectionParams in sectionSpawnParams)
        {
            foreach (GameObject obj in sectionParams.spawnedObjects.ToArray()) // Use ToArray() to avoid modification of the collection while iterating
            {
                if (Vector3.Distance(obj.transform.position, player.position) > despawnRadius)
                {
                    sectionParams.spawnedObjects.Remove(obj);
                    Destroy(obj);
                    totalSpawnedObjects--; // Decrement total spawned objects count
                }
            }
        }
    }

    SectionSpawnParams GetCurrentSection(Vector3 position)
    {
        foreach (SectionSpawnParams sectionParams in sectionSpawnParams)
        {
            GameObject sectionGameObject = sectionParams.sectionGameObject;
            Bounds sectionBounds = sectionGameObject.GetComponent<Collider>().bounds;

            if (sectionBounds.Contains(position))
            {
                return sectionParams;
            }
        }

        return null;
    }

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
