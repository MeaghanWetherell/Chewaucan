using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Scriptable objects that stores the different types of movement based sounds
 * (walking, jumping, landing). 
 */
[CreateAssetMenu]
public class MovementSounds : ScriptableObject
{
    public List<AudioClip> stepSounds = new List<AudioClip>();
    public List<AudioClip> jumpSounds = new List<AudioClip>();
    public List<AudioClip> landSounds = new List<AudioClip>();
    public List<AudioClip> sprintSounds = new List<AudioClip>();
}
