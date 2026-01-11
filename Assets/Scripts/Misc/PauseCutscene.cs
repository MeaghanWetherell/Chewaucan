using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.Playables;

public class PauseCutscene : MonoBehaviour
{
    public PlayableDirector cutscene;

    private void OnEnable()
    {
        PauseCallback.pauseManager.SubscribeToPause(Pause);
        PauseCallback.pauseManager.SubscribeToResume(Resume);
    }

    private void OnDisable()
    {
        PauseCallback.pauseManager.UnsubToPause(Pause);
        PauseCallback.pauseManager.UnsubToResume(Resume);
    }

    private void Pause()
    {
        cutscene.Pause();
    }

    private void Resume()
    {
        cutscene.Resume();
    }
}
