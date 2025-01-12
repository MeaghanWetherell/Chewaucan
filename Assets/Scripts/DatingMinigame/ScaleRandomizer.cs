using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRandomizer : MonoBehaviour
{
    public float scaleMinX;

    public float scaleMaxX;

    public float scaleMinY;

    public float scaleMaxY;

    public float scaleMinZ;

    public float scaleMaxZ;
    private void Awake()
    {
        float scaleX = Random.Range(scaleMinX, scaleMaxX);
        float scaleY = Random.Range(scaleMinY, scaleMaxY);
        float scaleZ = Random.Range(scaleMinZ, scaleMaxZ);
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x * scaleX, scale.y * scaleY, scale.z * scaleZ);
    }
}
