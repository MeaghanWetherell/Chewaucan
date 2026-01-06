using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class SetSubtitles : MonoBehaviour
{
    public void ChangeSubtitles()
    {
        SoundManager.soundManager.subtitlesOn = !SoundManager.soundManager.subtitlesOn;
    }

    public void ChangeSubtitles(bool change)
    {
        SoundManager.soundManager.subtitlesOn = change;
    }
}
