using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SplineAlignToTerrain))]
public class SplineAlignToTerrainEditor : Editor
{
    private SplineAlignToTerrain adjustSpline;

    private void OnEnable()
    {
        adjustSpline = (SplineAlignToTerrain)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Align to Terrain"))
        {
            adjustSpline = (SplineAlignToTerrain)target;
            adjustSpline.AlignToTerrain();
        }
    }

    private void OnSceneGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
        }
    }
}
