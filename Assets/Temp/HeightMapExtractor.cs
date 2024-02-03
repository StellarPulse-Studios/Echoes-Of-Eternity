using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HeightMapExtractor : MonoBehaviour
{
    public Terrain terrain;

    [ContextMenu("Extract Height Map")]
    private void ExtractHeights()
    {
        TerrainData data = terrain.terrainData;

        int mapSize = data.heightmapResolution;
        float[,] heights = data.GetHeights(0, 0, mapSize, mapSize);
        Color[] colors = new Color[mapSize * mapSize];

        float min = float.MaxValue;
        float max = float.MinValue;

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                float value = heights[x, y];
                if (value < min)
                    min = value;
                if (value > max)
                    max = value;
            }
        }

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                colors[y * mapSize + x] = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(min, max, heights[y, x]));
            }
        }

        Texture2D texture = new Texture2D(mapSize, mapSize, TextureFormat.R8, true);
        texture.SetPixels(colors);
        texture.Apply();
        
        byte[] bytes = texture.EncodeToPNG();
        DestroyImmediate(texture);

        string path = UnityEditor.EditorUtility.SaveFilePanel("Save to", "", "HeightMap", "png");
        print(path);
        File.WriteAllBytes(path, bytes);
    }
}
