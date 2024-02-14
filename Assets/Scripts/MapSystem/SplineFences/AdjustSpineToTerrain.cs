using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using Unity.VisualScripting;

[ExecuteInEditMode]
[RequireComponent(typeof(SplineContainer))]
public class AdjustSpineToTerrain : MonoBehaviour
{
    private SplineContainer splineContainer;
    private Spline spline;
    private int knotCount;

    [Tooltip("The space between knots. Lower number = more accurate")]
    [SerializeField] int knotInterval = 10;

    [SerializeField] int splineToAlign = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        splineContainer = GetComponent<SplineContainer>();
        spline = splineContainer[splineToAlign];
        knotCount = spline.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            if (spline != null && spline.Count != knotCount)
            {
                knotCount = spline.Count;
                Vector3 knotPos = splineContainer.transform.position + (Vector3)spline.ToArray()[spline.Count-1].Position;
                Terrain terrain = getActualCurrentTerrain(knotPos);
                Debug.Log("A new knot was added");
            }
            /*
            if (knotTarget != null)
            {
                var firstKnot = spline.ToArray()[0];

                firstKnot.Position = splineContainer.transform.InverseTransformPoint(knotTarget.position);
                firstKnot.Rotation = Quaternion.Inverse(splineContainer.transform.rotation) * knotTarget.rotation;

                spline.SetKnot(0, firstKnot);
            }*/
        }
    }

    private void AlignKnots(Terrain terrain)
    {
        BezierKnot[] knots = spline.ToArray();
        BezierKnot start = knots[spline.Count-2];
        BezierKnot end = knots[spline.Count - 1];

        Vector3 splinePos = splineContainer.transform.position;
        Vector3 startPos = splinePos + (Vector3)start.Position;
        Vector3 endPos = splinePos + (Vector3)end.Position;
    }

    private Terrain getActualCurrentTerrain(Vector3 pos)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(pos, Vector3.up, Mathf.Infinity);

        foreach (RaycastHit hit in hits)
        {
            Terrain terrain = hit.collider.gameObject.GetComponent<Terrain>();
            if (terrain != null)
            {
                Debug.Log(terrain.name);
                return terrain;
            }
        }

        return null;
    }

    public void AlignToTerrain()
    {
        Debug.Log("ALIGNING TERRAIN");
    }
}
