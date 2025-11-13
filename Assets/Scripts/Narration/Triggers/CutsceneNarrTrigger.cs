using System;
using System.Collections;
using System.Collections.Generic;
using Narration;
using UnityEngine;

public class CutsceneNarrTrigger : MonoBehaviour
{
    public Narration.Narration narrToPlay;
    
    private void OnEnable()
    {
        narrToPlay.Begin(false);
    }

    private void OnDisable()
    {
        narrToPlay.Stop();
    }
}
