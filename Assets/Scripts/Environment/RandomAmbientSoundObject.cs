using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RandomAmbientSoundObject : ScriptableObject
{
    public AudioClip audioClip;

    [Tooltip("What percent of the time this sound will play")]
    [Range(0f, 1f)]
    public float frequency;
}
