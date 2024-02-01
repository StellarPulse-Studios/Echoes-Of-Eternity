using UnityEngine;
using System.Collections.Generic;

public class ItemSpawnerManager : MonoBehaviour
{
    public GameObject player; // The player GameObject to calculate distances
    public LayerMask groundMask; // The ground layer mask to check for ground collisions
    public float activationRadius = 10f; // The radius within which items should be activated
    public List<SectionInfo> spawnSections; // List of sections where items should spawn
    public List<string> disallowedLayers; // List of layers where items should not spawn

    private List<GameObject> spawnedItems = new List<GameObject>(); // List to store spawned items

    [System.Serializable]
    public class ItemInfo
    {
        public GameObject itemPrefab; // The prefab of the item to spawn
        public int itemCount; // The number of items to spawn
    }

    [System.Serializable]
    public class SectionInfo
    {
        public GameObject sectionObject; // The section GameObject where items will be spawned
        public List<ItemInfo> items; // List of items to spawn in this section
    }

    void Start()
    {
        // Initialize and spawn items when the script starts
        SpawnItems();
    }

    void Update()
    {
        // Update is called every frame
        // Disable items outside the activation radius
        DisableItemsOutsideRadius();
        // Enable items inside the activation radius
        EnableItemsInsideRadius();
    }

    void SpawnItems()
    {
        // Loop through each section and spawn items
        foreach (SectionInfo section in spawnSections)
        {
            foreach (ItemInfo itemInfo in section.items)
            {
                // Spawn the specified number of items for each item type in the section
                for (int i = 0; i < itemInfo.itemCount; i++)
                {
                    // Get a random position within the section
                    Vector3 randomPosition = GetRandomPositionInSection(section.sectionObject);
                    GameObject itemToSpawn = itemInfo.itemPrefab;

                    // Check if the item prefab is not null
                    if (itemToSpawn != null)
                    {
                        // Find a valid spawn point within the section
                        randomPosition = FindValidSpawnPoint(randomPosition);

                        // Check if a valid spawn point was found
                        if (randomPosition != Vector3.zero)
                        {
                            // Instantiate the item at the spawn point and add it to the list of spawned items
                            GameObject spawnedItem = Instantiate(itemToSpawn, randomPosition, Quaternion.identity);
                            spawnedItem.SetActive(false);
                            spawnedItems.Add(spawnedItem);
                        }
                    }
                }
            }
        }
    }

    Vector3 GetRandomPositionInSection(GameObject sectionObject)
    {
        // Get a random position within the bounds of the section's collider
        Collider sectionCollider = sectionObject.GetComponent<Collider>();
        if (sectionCollider != null)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(sectionCollider.bounds.min.x, sectionCollider.bounds.max.x),
                Random.Range(sectionCollider.bounds.min.y, sectionCollider.bounds.max.y),
                Random.Range(sectionCollider.bounds.min.z, sectionCollider.bounds.max.z)
            );
            return randomPoint;
        }
        else
        {
            // Log an error if the section object does not have a collider
            Debug.LogError("Section object does not have a collider!");
            return Vector3.zero;
        }
    }

    Vector3 FindValidSpawnPoint(Vector3 initialPoint)
    {
        // Attempt to find a valid spawn point within a certain distance
        Vector3 spawnPoint = initialPoint;
        for (int i = 0; i < 10; i++) // Try a maximum of 10 times to find a valid spawn point
        {
            // Check if the current spawn point is valid
            if (CanSpawnAtPosition(spawnPoint))
            {
                return spawnPoint;
            }
            else
            {
                // Try a new random point around the initial point
                spawnPoint = GetRandomPointAround(initialPoint, 1f); // Adjust the distance as needed
            }
        }
        // Log a warning if a valid spawn point is not found after 10 attempts
        Debug.LogWarning("Unable to find a valid spawn point after 10 attempts.");
        return Vector3.zero;
    }

    Vector3 GetRandomPointAround(Vector3 center, float distance)
    {
        // Get a random point around a center point within a specified distance
        Vector2 randomPoint = Random.insideUnitCircle.normalized * distance;
        return new Vector3(center.x + randomPoint.x, center.y, center.z + randomPoint.y);
    }

    bool CanSpawnAtPosition(Vector3 position)
    {
        // Check if an item can spawn at the given position
        // Check if there's any collider at the position
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);
        foreach (Collider collider in colliders)
        {
            // Check if the collider is not a trigger and is on any disallowed layer
            if (!collider.isTrigger && disallowedLayers.Contains(LayerMask.LayerToName(collider.gameObject.layer)))
            {
                return false;
            }
        }
        // Return true if no obstacles or disallowed layers are found at the position
        return true;
    }

    void EnableItemsInsideRadius()
    {
        // Enable items within the activation radius of the player
        foreach (GameObject item in spawnedItems)
        {
            if (!item.activeSelf && Vector3.Distance(item.transform.position, player.transform.position) < activationRadius)
            {
                item.SetActive(true);
            }
        }
    }

    void DisableItemsOutsideRadius()
    {
        // Disable items outside the activation radius of the player
        foreach (GameObject item in spawnedItems)
        {
            if (item.activeSelf && Vector3.Distance(item.transform.position, player.transform.position) > activationRadius)
            {
                item.SetActive(false);
            }
        }
    }
}
