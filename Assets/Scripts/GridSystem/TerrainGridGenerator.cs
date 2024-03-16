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
    public float angleThreshold = 40.0f;
    public float minHeight = 70.0f;
    public float maxHeight = 140.0f;

    // Now the dictionary is public so other scripts can access it
    public Dictionary<string, List<Vector3>> sectionGridPointsMap { get; private set; }

    // Public field for the layer mask
    public LayerMask groundMask;

#if UNITY_EDITOR
    [ContextMenu("Generate Terrain Grid")]
    void GenerateTerrainGrid()
    {
        sectionGridPointsMap = new Dictionary<string, List<Vector3>>();

        // Iterate through each section
        foreach (GameObject section in sections)
        {
            sectionGridPointsMap.Add(section.name, new List<Vector3>());
        }

        // Get terrain data
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainSize = terrainData.size;

        // Calculate grid size
        int gridXCount = Mathf.FloorToInt(terrainSize.x / gridSize);
        int gridZCount = Mathf.FloorToInt(terrainSize.z / gridSize);
        int count = 0;
        int count1 = 0;
        int count2 = 0;

        // Break the terrain into grid and assign each grid point to its section
        for (int x = -gridXCount/2; x <= gridXCount/2; x++)
        {
            for (int z = -gridZCount/2; z <= gridZCount/2; z++)
            {
                Vector3 gridPoint = new Vector3(x * gridSize, 0, z * gridSize);

                // Use a raycast with the ground layer mask
                RaycastHit[] hits = Physics.RaycastAll(gridPoint + Vector3.up * 10000f, Vector3.down, Mathf.Infinity, groundMask, QueryTriggerInteraction.Collide);
                foreach(var hit in hits)
                {
                    GameObject section = hit.collider.gameObject;
                    if (sectionGridPointsMap.ContainsKey(section.name) == false) continue;
                    if (Physics.Raycast(hit.point + Vector3.down, Vector3.down, out RaycastHit cast, Mathf.Infinity, groundMask))
                    {
                        if (Vector3.Angle(cast.normal, Vector3.up) > angleThreshold) continue;
                        if (cast.point.y < minHeight || cast.point.y > maxHeight) continue;
                        sectionGridPointsMap[section.name].Add(cast.point);
                    }
                }

            }
        }

        // Output the count of points that couldn't be assigned to a section
        Debug.Log("Count: " + count);
        Debug.Log("Count1: " + count1);
        Debug.Log("Count2: " + count2);

        // Output the count of points assigned to each section
        foreach (KeyValuePair<string, List<Vector3>> pair in sectionGridPointsMap)
        {
            Debug.Log(pair.Key + " " + pair.Value.Count);
        }

        // Save the dictionary to a JSON file
        //SaveDictionaryAsJson("SectionGridPoints.json", sectionGridPointsMap);
    }
    private void OnDrawGizmos()
    {
        if (sectionGridPointsMap==null) {
            return;
        }
        int id = 1;
        foreach(var value in sectionGridPointsMap)
        {
            if (id == sectionId)
            {
                foreach (var point in value.Value)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(point, 0.5f);
                }
                
            }
            id++;
        }
    }
#endif

    // Other methods remain unchanged
}
