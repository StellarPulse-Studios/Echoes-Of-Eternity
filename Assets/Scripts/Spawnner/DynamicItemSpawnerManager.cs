using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class DynamicItemSpawnerManager : StaticItemSpawnerManager

{
    public int respawnAmount = 0;

    private Dictionary<GameObject, GameObject> ItemsToRespawn = new Dictionary<GameObject, GameObject>();


    protected override void Update()
    {
        // Update is called every frame
        if (ItemsToRespawn.Count >= respawnAmount)
        {
            SpawnItems(ItemsToRespawn);
        }
        base.Update();
    }

    protected void SpawnItems(Dictionary<GameObject, GameObject> respawnItems)
    {
        foreach (var item in respawnItems)
        {
            Vector3 randomSpawnPoint = GetRandomSpawnPoint(item.Key);

            GameObject itemToSpawn = item.Value;

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

    public void AddItemToDespawn(GameObject section, GameObject item)
    {
        ItemsToRespawn.Add(section, item);
    }

    private void OnDrawGizmos()
    {
        float mindistance = float.MaxValue;
        Vector3 playerPos = player.transform.position;
        GameObject itemToDraw = null;
        foreach (var item in spawnedItems)
        {
            Vector3 itemPos = item.transform.position;
            float pos = Vector3.Distance(itemPos, playerPos);
            if (mindistance > pos)
            {
                mindistance = pos;
                itemToDraw = item;
            }
        }
        Gizmos.color = Color.red;
        if (itemToDraw != null)
            Gizmos.DrawLine(playerPos, itemToDraw.transform.position);
        if (player != null)
            UnityEditor.Handles.DrawWireArc(playerPos, Vector3.up, player.transform.forward, 360.0f, activationRadius);

    }

}
