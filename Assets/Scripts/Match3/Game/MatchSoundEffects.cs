using System.Collections.Generic;
using Audio;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3.Game
{
    //plays sound effects as needed in match 3
    public class MatchSoundEffects : MonoBehaviour
    {
        [Tooltip("Ref to the audio source for matches")] public AudioSource matchAud;

        [Tooltip("All 'yay' sounds that can play")] public List<AudioClip> yayAud;

        [Tooltip("All 'Aw' sounds that can play")] public List<AudioClip> awAud;

        [Tooltip("BGM to play during match 3")] public List<AudioClip> bgm;

        [Tooltip("Degree to quiet BGM")] public float BGMattenuation;

        private void Awake()
        {
            SoundManager.soundManager.SetBGM(bgm);
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
            if (!SoundManager.soundManager.IsMuted(2))
            {
                StartCoroutine(SoundManager.soundManager.QuietBGMUntilDone(matchAud, BGMattenuation));
            }
            matchAud.Stop();
            matchAud.clip = temp;
            matchAud.Play();
        }
    }
}
