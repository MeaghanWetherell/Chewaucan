using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CourseTimer : MonoBehaviour
{
    private float currentTime;

    private bool paused = true;

    [Tooltip("Ref to the time display text")]
    public TextMeshProUGUI text;

    //called with true when the timer runs out, false when it stops otherwise    
    [NonSerialized]public UnityEvent<bool> timerStopped = new UnityEvent<bool>();

    //inits the time with the passed time
    public void SetTimer(float time)
    {
        currentTime = time;
        paused = false;
        text.gameObject.SetActive(true);
        SetText();
    }
    
    public void StopTimer()
    {
        StopTimer(false);
    }
    
    //stops the timer, invokes timerStopped
    private void StopTimer(bool status)
    {
        text.gameObject.SetActive(false);
        paused = true;
        timerStopped.Invoke(status);
    }

    private void Update()
    {
        if(!paused && !PauseCallback.pauseManager.isPaused)
            currentTime -= Time.deltaTime;
        if (currentTime <= 0)
            StopTimer(true);
        else
            SetText();
    }

    private void SetText()
    {
        if(currentTime % 60 < 10)
            text.text = "" + ((int)currentTime / 60)+":0"+((int)currentTime % 60);
        else
            text.text = "" + ((int)currentTime / 60)+":"+((int)currentTime % 60);
    }
}
