using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

//Determines automatically whether subtitles are on or off and enables the appropriate button
public class SubtitleButtonEnabler : MonoBehaviour
{
    public GameObject check;

    public GameObject uncheck;

    void Start()
    {
        if (SoundManager.soundManager.subtitlesOn)
        {
            check.SetActive(true);
            uncheck.SetActive(false);
        }
        else
        {
            check.SetActive(false);
            uncheck.SetActive(true);
        }
    }
}
