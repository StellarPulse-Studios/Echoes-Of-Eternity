using UnityEngine;
using System.Collections.Generic;

public class StaticItemSpawnerManager : MonoBehaviour
{
    public GameObject player; // The player GameObject to calculate distances
    public float activationRadius = 10f; // The radius within which items should be activated
    public List<SectionInfo> spawnSections; // List of sections where items should spawn

    public List<GameObject> spawnedItems = new List<GameObject>(); // List to store spawned items

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

    void Awake()
    {
        // Initialize and spawn items when the script starts
        SpawnItems();
    }

    protected virtual void Update()
    {
        // Update is called every frame
        // Disable items outside the activation radius
        DisableItemsOutsideRadius();
        // Enable items inside the activation radius
        EnableItemsInsideRadius();
    }

    protected void SpawnItems()
    {
        // Loop through each section and spawn items
        foreach (SectionInfo section in spawnSections)
        {
            foreach (ItemInfo itemInfo in section.items)
            {
                // Spawn the specified number of items for each item type in the section
                for (int i = 0; i < itemInfo.itemCount; i++)
                {
                    // Get a random spawn point within the section
                    Vector3 randomSpawnPoint = GetRandomSpawnPoint(section.sectionObject);

                    GameObject itemToSpawn = itemInfo.itemPrefab;

                    // Check if the item prefab is not null
                    if (itemToSpawn == null)
                    {
                        continue;
                    }

                    // Spawn the item at the random spawn point
                    GameObject spawnedItem = Instantiate(itemToSpawn, randomSpawnPoint, Quaternion.identity);
                    spawnedItems.Add(spawnedItem);
                }
            }
        }
    }

    protected Vector3 GetRandomSpawnPoint(GameObject sectionObject)
    {
        SectionData sectionData = sectionObject.GetComponent<SectionData>();

        if (sectionData != null && sectionData.spawnPoints != null && sectionData.spawnPoints.points.Count > 0)
        {
            // Get a random spawn point from the list of points in SectionSpawnPointSo
            int randomIndex = Random.Range(0, sectionData.spawnPoints.points.Count);
            return sectionData.spawnPoints.points[randomIndex];
        }
        else
        {
            // Log an error if the section data or spawn points are not set up properly
            Debug.LogError("Section data or spawn points are not set up properly.");
            return Vector3.zero;
        }
    }

    protected void EnableItemsInsideRadius()
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

    protected void DisableItemsOutsideRadius()
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

    public void RemoveItem(GameObject item)
    {
        // Remove the item from the spawned items list
        spawnedItems.Remove(item);
    }
}
