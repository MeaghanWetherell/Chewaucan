using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public class ClipListPlayer : MonoBehaviour
{
    public AudioSource mySource;

    private List<DragTarget> lastTargets = new List<DragTarget>();

    private List<bool> lastEng;

    public void PlayClips(List<AudioClip> clips, List<DragTarget> targets, List<bool> isEnglish)
    {
        Stop();
        lastTargets = targets;
        lastEng = isEnglish;
        StartCoroutine(PlayCoroutine(clips, targets, isEnglish));
    }

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
