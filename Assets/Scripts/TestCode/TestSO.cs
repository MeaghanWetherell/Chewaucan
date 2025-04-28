using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "test", fileName = "test")]
public class TestSO : ScriptableObject
{
    public Dictionary<string, Sprite> dict;
}
