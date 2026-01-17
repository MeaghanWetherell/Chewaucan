using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LakeLevelData : ScriptableObject
{
    [Tooltip("The y min for each level, in order, of the plateau game")]
    public List<float> yMin;

    [Tooltip("The y max for each level, in order, of the plateau game")]
    public List<float> yMax;

    [Tooltip("The date min for each level, in order, of the plateau game")]
    public List<int> dateMin;

    [Tooltip("The date max for each level, in order, of the plateau game")]
    public List<int> dateMax;
}
