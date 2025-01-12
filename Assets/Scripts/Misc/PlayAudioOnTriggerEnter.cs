using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class PlayAudioOnTriggerEnter : MonoBehaviour
{
    public AudioSource src;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
            src.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        src.Stop();
    }
}
