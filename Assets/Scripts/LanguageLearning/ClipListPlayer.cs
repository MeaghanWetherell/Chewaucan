using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public class ClipListPlayer : MonoBehaviour
{
    [Tooltip("Audio source to play from")]
    public AudioSource mySource;
 
    //these private variables are used for accurately stopping the last set of clips when a new one is called to play
    //the targets that represent the words we're playing, in order
    private List<DragTarget> lastTargets = new List<DragTarget>();

    //stores whether the clip at each index in the clip list currently playing is an english clip or an alt lang clip
    private List<bool> lastEng;

    //stops playing previous clips and plays the new list. targets and isEnglish should match the format expected by the private variables abov
    public void PlayClips(List<AudioClip> clips, List<DragTarget> targets, List<bool> isEnglish)
    {
        Stop();
        lastTargets = targets;
        lastEng = isEnglish;
        StartCoroutine(PlayCoroutine(clips, targets, isEnglish));
    }

    //stops any clips that were playing
    public void Stop()
    {
        mySource.Stop();
        StopAllCoroutines();
        int i = 0;
        foreach (DragTarget targ in lastTargets)
        {
            targ.Dehighlight(lastEng[i]);
            i++;
        }
        lastTargets = new List<DragTarget>();
    }

    //play the clips in order
    private IEnumerator PlayCoroutine(List<AudioClip> clips, List<DragTarget> targets, List<bool> isEnglish)
    {
        int i = 0;
        foreach (AudioClip clip in clips)
        {
            float time = clip.length;
            mySource.clip = clip;
            mySource.Play();
            targets[i].Highlight(isEnglish[i]);
            while (time > 0)
            {
                if (!PauseCallback.pauseManager.isPaused)
                {
                    time -= Time.deltaTime;
                    yield return new WaitForSeconds(0);
                }
            }
            mySource.Stop();
            targets[i].Dehighlight(isEnglish[i]);
            i++;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
