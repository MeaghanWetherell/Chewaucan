using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;

public class DemoStartNarration : MonoBehaviour
{
    public AudioClip startNarration;

    private void Start()
    {
        Player.player.GetComponent<LandMovement>().enabled = false;
        List<UnityAction> onComplete = new List<UnityAction>();
        onComplete.Add(OnComplete);
        SoundManager.soundManager.PlayNarration(startNarration, onComplete);
    }

    private void OnComplete()
    {
        Player.player.GetComponent<LandMovement>().enabled = true;
    }
}
