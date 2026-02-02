using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SentenceManager : MonoBehaviour
{
    [NonSerialized]public Sentence mySentence;

    public TextMeshProUGUI myText;

    public GameObject myAudioSource;

    public void Init(Sentence inSent, AudioClip clip, HashSet<string> unlockedIds)
    {
        mySentence = inSent;
        if(clip == null)
            Destroy(myAudioSource);
        else
        {
            myAudioSource.GetComponent<AudioSource>().clip = clip;
        }
        Refresh(unlockedIds);
    }

    public void Refresh(HashSet<string> unlockedIds)
    {
        (String, bool) tup = mySentence.GetSentenceVal(unlockedIds);
        myText.text = tup.Item1;
        if(myAudioSource != null)
            myAudioSource.SetActive(tup.Item2);
    }
}
