/*using System.Collections.Generic;
using UnityEngine;

public class SomeOtherScript : MonoBehaviour
{
    public TerrainGridGenerator gridGenerator;

    void Start()
    {
        if (gridGenerator != null)
        {
            Dictionary<GameObject, List<Vector3>> gridPointsMap = gridGenerator.sectionGridPointsMap;

            // Now you can use the gridPointsMap dictionary as needed
            foreach (KeyValuePair<GameObject, List<Vector3>> kvp in gridPointsMap)
            {
                GameObject section = kvp.Key;
                List<Vector3> gridPoints = kvp.Value;

                Debug.Log("Section: " + section.name);
                foreach (Vector3 point in gridPoints)
                {
                    Debug.Log("Grid Point: " + point);
                }
            }
        }
    }
}*/
