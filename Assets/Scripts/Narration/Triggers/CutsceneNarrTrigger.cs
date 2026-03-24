using System;
using System.Collections;
using System.Collections.Generic;
using Narration;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneNarrTrigger : MonoBehaviour
{
    [Tooltip("Narration to play when triggered")]
    public Narration.Narration narrToPlay;

    [Tooltip("Narration to trigger automatically when this one completes")]
    public CutsceneNarrTrigger nextNarr;
    
    public void StartNarr()
    {
        narrToPlay.Begin(new List<UnityAction<string>>{OnNarrEnd},false);
    }

    public void StopNarr()
    {
        narrToPlay.Stop();
    }

    private void OnNarrEnd(string clip)
    {
        if(nextNarr != null)
            StartCoroutine(EnableNextNarr());
    }

    private IEnumerator EnableNextNarr()
    {
        yield return new WaitForSeconds(0.2f);
        nextNarr.StartNarr();
    }
}
