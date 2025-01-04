using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;

public class CourseManager : MonoBehaviour
{
    public Transform startPosition;

    public float datingPointsToWin;

    [NonSerialized]public float curPoints;

    [NonSerialized]public bool active;

    public float courseTime;

    public CourseTimer timer;

    [NonSerialized]public UnityEvent Started = new UnityEvent();

    [NonSerialized]public UnityEvent Stopped = new UnityEvent();
    
    public void StartCourse()
    {
        timer.SetTimer(courseTime);
        timer.timerStopped.AddListener(Reset);
        active = true;
        Started.Invoke();
    }

    public void Reset()
    {
        Player.player.GetComponent<CharacterController>().enabled = false;
        Player.player.transform.position = startPosition.position;
        Player.player.transform.eulerAngles = startPosition.eulerAngles;
        Player.player.GetComponent<CharacterController>().enabled = true;
        curPoints = 0;
        active = false;
        timer.StopTimer();
        Stopped.Invoke();
    }

    private void Reset(bool val)
    {
        if(val)
            Reset();
    }

    public void AddPoints(float points)
    {
        curPoints += points;
    }
}
