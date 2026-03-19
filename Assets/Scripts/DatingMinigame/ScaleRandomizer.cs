using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//randomizes this object's scale on start
public class ScaleRandomizer : MonoBehaviour
{
    [Tooltip("Min x scale for random scale")]public float scaleMinX;

    [Tooltip("Max x scale for random scale")]public float scaleMaxX;

    [Tooltip("Min y scale for random scale")]public float scaleMinY;

    [Tooltip("Max y scale for random scale")]public float scaleMaxY;

    [Tooltip("Min z scale for random scale")]public float scaleMinZ;

    [Tooltip("Max z scale for random scale")]public float scaleMaxZ;
    private void Awake()
    {
        float scaleX = Random.Range(scaleMinX, scaleMaxX);
        float scaleY = Random.Range(scaleMinY, scaleMaxY);
        float scaleZ = Random.Range(scaleMinZ, scaleMaxZ);
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x * scaleX, scale.y * scaleY, scale.z * scaleZ);
    }
}
