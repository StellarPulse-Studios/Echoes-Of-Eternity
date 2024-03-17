using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TerrainGridGenerator : MonoBehaviour
{
    public GameObject[] sections;
    public float gridSize = 1f;
    public int sectionId = 1;
    public GameObject building;


    // Now the dictionary is public so other scripts can access it
    public Dictionary<string, SectionData> sectionGridPointsMap { get; private set; }

    // Public field for the layer mask
    public LayerMask spawnableMask;
    public LayerMask sectionMask;

#if UNITY_EDITOR
    [ContextMenu("Generate Terrain Grid")]
    void GenerateTerrainGrid()
    {
        sectionGridPointsMap = new Dictionary<string, SectionData>();

        // Iterate through each section
        foreach (GameObject section in sections)
        {
            section.SetActive(true);
            SectionData data = section.GetComponent<SectionData>();
            sectionGridPointsMap.Add(section.name, data);
            data.spawnPoints.points.Clear();
        }

        // Get terrain data
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        // Calculate grid size
        int gridXCount = Mathf.FloorToInt(terrainSize.x / gridSize);
        int gridZCount = Mathf.FloorToInt(terrainSize.z / gridSize);
        int count = 0;


        // Break the terrain into grid and assign each grid point to its section
        for (int x = -gridXCount/2; x <= gridXCount/2; x++)
        {
            for (int z = -gridZCount/2; z <= gridZCount/2; z++)
            {
                Vector3 gridPoint = new Vector3(x * gridSize, 0, z * gridSize);

                // Use a raycast with the ground layer mask
                RaycastHit[] hitSections = Physics.RaycastAll(gridPoint + Vector3.up * 10000f, Vector3.down, Mathf.Infinity, sectionMask, QueryTriggerInteraction.Collide);
                foreach(var hitSection in hitSections)
                {
                    count++;
                    GameObject section = hitSection.collider.gameObject;
                    if (sectionGridPointsMap.ContainsKey(section.name) == false) continue;
                    
                    if (Physics.Raycast(hitSection.point + Vector3.down, Vector3.down, out RaycastHit hitSpawnable, Mathf.Infinity))
                    {
                        if ((spawnableMask.value & (1 << (hitSpawnable.collider.gameObject.layer))) == 0) break;
                  
                        var data = sectionGridPointsMap[section.name];
                        if (Vector3.Angle(hitSpawnable.normal, Vector3.up) > data.thresholdAngle) continue;
                        if (hitSpawnable.point.y < data.minHeight || hitSpawnable.point.y > data.maxHeight) continue;
                        if (hitSpawnable.point.y < data.minHeight || hitSpawnable.point.y > data.maxHeight) continue;
                        data.spawnPoints.points.Add(hitSpawnable.point);
                        print(section.name);

                    }
                }

            }
        }

        // Output the count of points that couldn't be assigned to a section
        Debug.Log("Count: " + count);

        // Output the count of points assigned to each section
        foreach (var pair in sectionGridPointsMap)
        {
            Debug.Log(pair.Key + " " + pair.Value.spawnPoints.points.Count);
        }

        // Save the dictionary to a JSON file
        //SaveDictionaryAsJson("SectionGridPoints.json", sectionGridPointsMap);
    }
    private void OnDrawGizmos()
    {
        int id = 1;
        foreach (var section in sections)
        {
            if (id == sectionId)
            {
                SectionData data = section.GetComponent<SectionData>();
                if (data.spawnPoints == null) continue;
                foreach (var point in data.spawnPoints.points)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(point, 0.5f);
                }
            }
            id++;

        }
        
    }
    [ContextMenu("Mask")]
    private void PrintLayer()
    {
        for(int i = 0; i < 32; i++)
        {
            print((spawnableMask.value & (1 << i)) == 0);
            
        }
        print(building.layer);
    }
#endif

}
