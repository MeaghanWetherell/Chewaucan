using System;
using System.Collections;
using System.Collections.Generic;
using Narration;
using QuestSystem;
using UnityEngine;
using UnityEngine.Events;

public class WallUntilNarrComplete : MonoBehaviour
{
    public Narration.Narration waitForNarr;
    
    private void Start()
    {
        if (waitForNarr.HasPlayed())
        {
            Destroy(gameObject);
        }
        else
        {
            waitForNarr.addToOnComplete(new List<UnityAction<string>>() {NarrComplete});
        }
    }

    private void OnDisable()
    {
        waitForNarr.RemoveFromOnComplete(new List<UnityAction<string>>() {NarrComplete});
    }

    private void NarrComplete(string none)
    {
        Destroy(gameObject);
    }
}
