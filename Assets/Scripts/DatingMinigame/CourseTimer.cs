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

    public TextMeshProUGUI text;

    public UnityEvent<bool> timerStopped;

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
    
    private void StopTimer(bool status)
    {
        text.gameObject.SetActive(false);
        paused = true;
        timerStopped.Invoke(status);
        timerStopped = new UnityEvent<bool>();
    }

    private void Update()
    {
        if(!paused)
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

    private void Awake()
    {
        if (QuestManager.questManager != null) 
        {
            PauseCallback.pauseManager.SubscribeToPause(OnPause);
            PauseCallback.pauseManager.SubscribeToResume(OnResume);
            return;
        }
        int count = SceneManager.sceneCount;
        for (int i = 0; i < count; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Equals("PersistentObjects"))
            {
                PauseCallback.pauseManager.SubscribeToPause(OnPause);
                PauseCallback.pauseManager.SubscribeToResume(OnResume);
                return;
            }
        }
        LoadPersistentObjects.LoadObjs();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!scene.name.Equals("PersistentObjects")) return;
        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
        
    //unsubscribe to prevent leaks
    private void OnDisable()
    {
        PauseCallback.pauseManager.UnsubToPause(OnPause);
        PauseCallback.pauseManager.UnsubToResume(OnResume);
    }
    
    private void OnPause()
    {
        paused = true;
    }

    private void OnResume()
    {
        paused = false;
    }
}
