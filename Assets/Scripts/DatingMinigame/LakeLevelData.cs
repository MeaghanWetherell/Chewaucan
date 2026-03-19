using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores the data about each level in a convenient location accessible to all instances of course manager
//which allows all rocks to respond with the correct dates, even if they do not belong at the level of the current course manager
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
