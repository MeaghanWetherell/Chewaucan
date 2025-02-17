using UnityEngine;
using UnityEditor;
using System.IO;

public class TerrainHeightmapExporter : EditorWindow
{
    [MenuItem("Tools/Export All Terrain Heightmaps")]
    public static void ExportAllTerrains()
    {
        Terrain[] terrains = Terrain.activeTerrains; // Get all terrains in the scene

        if (terrains.Length == 0)
        {
            Debug.LogError("No terrains found in the scene!");
            return;
        }

        string folderPath = "Assets/Heightmap/";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        foreach (Terrain terrain in terrains)
        {
            ExportHeightmap(terrain, folderPath);
        }

        AssetDatabase.Refresh();
        Debug.Log($"check Exported {terrains.Length} heightmaps to {folderPath}");
    }

    private static void ExportHeightmap(Terrain terrain, string folderPath)
    {
        TerrainData terrainData = terrain.terrainData;
        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;
        float[,] heights = terrainData.GetHeights(0, 0, width, height);

        string terrainName = terrain.name.Replace(" ", "_");
        string savePath = $"{folderPath}{terrainName}.raw";

        byte[] rawData = new byte[width * height * 2]; // 16-bit grayscale
        int index = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                ushort heightValue = (ushort)(heights[y, x] * 65535);
                rawData[index++] = (byte)(heightValue & 0xFF); // Low byte
                rawData[index++] = (byte)((heightValue >> 8) & 0xFF); // High byte
            }
        }

        File.WriteAllBytes(savePath, rawData);
        Debug.Log($"check Heightmap exported: {savePath}");
    }
}
