using System;
using System.Collections;
using System.Collections.Generic;
using Narration;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneNarrTrigger : MonoBehaviour
{
    public Narration.Narration narrToPlay;

    public CutsceneNarrTrigger nextNarr;
    
    public void StartNarr()
    {
            //Debug.Log("Starting narr "+narrToPlay.name);
            narrToPlay.Begin(new List<UnityAction<string>>{OnNarrEnd},false);
    }

    public void StopNarr()
    {
        //Debug.Log("Stopping "+narrToPlay.name);
        narrToPlay.Stop();
    }

    private void OnNarrEnd(string clip)
    {
        if(nextNarr != null)
            StartCoroutine(EnableNextNarr());
    }

    private IEnumerator EnableNextNarr()
    {
        //Debug.Log("Waiting to enable narr "+nextNarr.name);
        yield return new WaitForSeconds(0.2f);
        //Debug.Log("Enabling "+nextNarr.name);
        nextNarr.StartNarr();
    }
}
