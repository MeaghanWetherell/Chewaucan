using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RandomAmbientSoundObject : ScriptableObject
{
    public AudioClip audioClip;

    [Tooltip("How often this sound will play")]
    [Range(0f, 1f)]
    public float frequency;
}
