using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutsceneOnTriggerEnter : MonoBehaviour
{
    public PlayableDirector cutscene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            cutscene.Play();
        }
    }
}
