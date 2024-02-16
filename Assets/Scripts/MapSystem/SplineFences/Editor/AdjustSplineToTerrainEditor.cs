using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(AdjustSpineToTerrain))]
public class AdjustSplineToTerrainEditor : Editor
{
    private AdjustSpineToTerrain adjustSpline;

    private void OnEnable()
    {
        adjustSpline = (AdjustSpineToTerrain)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Align to Terrain"))
        {
            adjustSpline = (AdjustSpineToTerrain)target;
            adjustSpline.AlignToTerrain();
        }

    }
}
