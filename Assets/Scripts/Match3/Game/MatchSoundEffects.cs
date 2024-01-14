using System;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3.Game
{
    public class MatchSoundEffects : MonoBehaviour
    {
        [Tooltip("Ref to the audio source for matches")] public AudioSource matchAud;

        [Tooltip("All 'yay' sounds that can play")] public List<AudioClip> yayAud;

        [Tooltip("All 'Aw' sounds that can play")] public List<AudioClip> awAud;

        [Tooltip("BGM to play during match 3")] public List<AudioClip> bgm;

        private void Awake()
        {
            BGMManager.bgmManager.SetBGM(bgm);
        }

        public void PlayAw()
        {
            PlayFromList(awAud);
        }

        public void PlayYay()
        {
            PlayFromList(yayAud);
        }
        
        public void PlayFromList(List<AudioClip> audList)
        {
            AudioClip temp = audList[Random.Range(0, audList.Count)];
            StartCoroutine(BGMManager.bgmManager.QuietBGMUntilDone(matchAud));
            matchAud.Stop();
            matchAud.clip = temp;
            matchAud.Play();
        }
    }
}
