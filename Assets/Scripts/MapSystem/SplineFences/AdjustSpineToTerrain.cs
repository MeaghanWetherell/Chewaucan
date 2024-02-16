using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Splines;
using System;

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
    void OnEnable()
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
            spline = splineContainer[splineToAlign];
            if (spline != null && spline.Count != knotCount)
            {
                knotCount = spline.Count;
                Vector3 knotPos = splineContainer.transform.position + (Vector3)spline.ToArray()[spline.Count-1].Position;
                Terrain terrain = getActualCurrentTerrain(knotPos);
                Debug.Log("A new knot was added");
                AlignKnots(terrain);
            }
        }
    }

    /**
     * adds several knots in between two knots to align the spline to the terrain
     */
    private void AlignKnots(Terrain terrain)
    {
        Debug.Log("ALIGN KNOTS CALLED");
        BezierKnot[] knots = spline.ToArray();
        BezierKnot start = knots[spline.Count-2];
        BezierKnot end = knots[spline.Count - 1];

        Vector3 splinePos = splineContainer.transform.position;
        Vector3 startPos = splinePos + (Vector3)start.Position;
        Vector3 endPos = splinePos + (Vector3)end.Position;

        //Debug.Log(startPos.ToString() + " " + endPos.ToString());

        float distance = Vector3.Distance(startPos, endPos);
        int knotsToAdd = Mathf.Abs(Mathf.FloorToInt(distance/knotInterval));

        //Debug.Log(distance + " " + knotsToAdd);

        Vector3 posStep = endPos - startPos;

        Vector3 position = startPos;
        //repeatedly insert knots and move the last knot to the end
        for (int i = 0; i < knotsToAdd; i++)
        {
            //Debug.Log(position.ToString());
            position = new Vector3(position.x+(posStep.x/knotsToAdd), position.y, position.z+(posStep.z/knotsToAdd));
            float terrainHeight = terrain.SampleHeight(position);
            position = new Vector3(position.x, terrainHeight, position.z);

            //set the last knot to the proper value
            Vector3 targetKnotPos = splineContainer.transform.InverseTransformPoint(position);
            BezierKnot lastKnot = spline[spline.Count-1];
            lastKnot.Position = targetKnotPos;
            spline.SetKnot(spline.Count-1, lastKnot);

            //add the end knot back at the end
            spline.Add(end);
        }
    }

    private Terrain getActualCurrentTerrain(Vector3 pos)
    {
        RaycastHit[] hits;
        Vector3 raycastPos = new Vector3(pos.x, 100f, pos.z);
        hits = Physics.RaycastAll(raycastPos, Vector3.down, Mathf.Infinity);

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
