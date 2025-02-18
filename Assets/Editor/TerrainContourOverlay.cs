using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TerrainContourOverlay : EditorWindow
{
    private static Material overlayMaterial;
    private static bool isOverlayActive = false;
    private static Dictionary<Terrain, Material> originalMaterials = new Dictionary<Terrain, Material>();

    [MenuItem("Tools/Toggle Terrain Contour Overlay")]
    public static void ToggleOverlay()
    {
        if (overlayMaterial == null)
        {
            Shader shader = Shader.Find("Custom/HeightmapOverlay");
            if (shader == null)
            {
                Debug.LogError("Shader 'Custom/HeightmapOverlay' not found! Make sure it's in Assets.");
                return;
            }
            overlayMaterial = new Material(shader);
        }

        Terrain[] terrains = Terrain.activeTerrains;
        if (terrains.Length == 0)
        {
            Debug.LogError("No terrains found in the scene!");
            return;
        }

        isOverlayActive = !isOverlayActive;

        foreach (Terrain terrain in terrains)
        {
            if (isOverlayActive)
            {
                // Store original material if not already stored
                if (!originalMaterials.ContainsKey(terrain))
                {
                    originalMaterials[terrain] = terrain.materialTemplate;
                }
                terrain.materialTemplate = overlayMaterial; // Apply overlay
            }
            else
            {
                // Restore original material
                if (originalMaterials.ContainsKey(terrain))
                {
                    terrain.materialTemplate = originalMaterials[terrain];
                }
            }
        }

        Debug.Log($"Terrain contour overlay {(isOverlayActive ? "enabled" : "disabled")}");
    }
}
