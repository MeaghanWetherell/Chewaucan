using System;
using System.Collections;
using System.Collections.Generic;
using Narration;
using UnityEngine;
using UnityEngine.Events;

public class CutsceneNarrTrigger : MonoBehaviour
{
    public Narration.Narration narrToPlay;

    public GameObject nextNarr;
    
    private void OnEnable()
    {
        narrToPlay.Begin(new List<UnityAction<string>>{OnNarrEnd},false);
    }

    private void OnDisable()
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
        nextNarr.SetActive(true);
    }
}
