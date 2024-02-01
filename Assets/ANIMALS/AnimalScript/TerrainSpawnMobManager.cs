using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainSpawnMobManager : MonoBehaviour
{
    [System.Serializable]
    public class SectionSpawnParams
    {
        public int maxCount;                             // Maximum number of objects to spawn in this section
        public List<GameObject> spawnPrefabs;           // List of game object prefabs to spawn
        public int[] spawnCounts;                        // Number of each prefab to spawn in this section
        public GameObject sectionGameObject;            // GameObject representing the section
        public List<GameObject> spawnedObjects;         // List to store spawned objects in this section
    }

    public SectionSpawnParams[] sectionSpawnParams;
    public Transform player;
    public LayerMask terrainLayer;
    public List<LayerMask> layersToAvoid; // List of layers to avoid spawning objects on
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
        if (totalSpawnedObjects >= GetMaxSpawnCap() || totalSpawnedObjects >= maxSpawnCount)
        {
            // Reset the spawn timer
            spawnTimer = spawnInterval;
        }
        if (spawnTimer <= 0f && totalSpawnedObjects < GetMaxSpawnCap() && totalSpawnedObjects < maxSpawnCount)
        {
            // Get a random spawn point around the player
            Vector3 spawnPoint = GetRandomSpawnPoint();

            // Check if the spawn point is valid
            if (IsValidSpawnPoint(spawnPoint))
            {
                // Spawn an object at the determined spawn point
                SpawnObjectAtPoint(spawnPoint);

                // Increment the total spawned objects count
                totalSpawnedObjects++;
            }
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

    bool IsValidSpawnPoint(Vector3 spawnPoint)
    {
        // Check if the spawn point is valid based on the layers to avoid
        Collider[] colliders = Physics.OverlapSphere(spawnPoint, 1f); // Check for nearby colliders
        foreach (Collider collider in colliders)
        {
            if (layersToAvoid.Contains(collider.gameObject.layer))
            {
                return false; // Invalid spawn point
            }
        }
        return true; // Valid spawn point
    }

    void SpawnObjectAtPoint(Vector3 spawnPoint)
    {
        // Find the section where the spawn point is located
        SectionSpawnParams currentSection = GetCurrentSection(spawnPoint);

        if (currentSection == null)
        {
            Debug.LogWarning("No section found for spawn point: " + spawnPoint);
            return;
        }

        // Randomly choose a prefab to spawn
        GameObject prefabToSpawn = currentSection.spawnPrefabs[Random.Range(0, currentSection.spawnPrefabs.Count)];

        // Check if we need to spawn more objects for this prefab
        int currentCount = currentSection.spawnedObjects.FindAll(obj => obj.name == prefabToSpawn.name).Count;
        if (currentCount >= currentSection.maxCount)
            return; // Maximum count reached for this prefab

        // Attempt to spawn around the spawn point
        int count = Mathf.Min(currentSection.spawnCounts.Length, currentSection.maxCount - currentCount);
        if (count > 0)
        {
            Vector3 randomOffset = Random.insideUnitSphere * spawnOuterRadius;
            randomOffset.y = 0f; // Keep the y-offset zero initially

            // Cast a ray from the spawn point downwards to find the terrain hit point
            RaycastHit hit;
            if (Physics.Raycast(spawnPoint + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
            {
                // Adjust spawn position to the terrain hit point
                Vector3 spawnPosition = hit.point + randomOffset;

                // Instantiate the prefab at the adjusted spawn position
                GameObject obj = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                obj.GetComponent<MobBlackboard>().Target = player.gameObject;
                currentSection.spawnedObjects.Add(obj);
            }
            else
            {
                Debug.LogWarning("No terrain found below spawn point: " + spawnPoint);
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

    int GetMaxSpawnCap()
    {
        int maxCap = 0;
        foreach (SectionSpawnParams sectionParams in sectionSpawnParams)
        {
            maxCap += sectionParams.maxCount;
        }
        return maxCap;
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
}

