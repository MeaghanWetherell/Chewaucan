using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Misc
{
    public class BGMManager : MonoBehaviour
    {
        public static BGMManager bgmManager;

        [Tooltip("Ref to BGM audio source")] public AudioSource bgm;

        [Tooltip("The volume to reduce the BGM to when quieted. must be from 0 to 1")]
        public float quietVol;

        [Tooltip("Standard volume of the BGM")] public float standVol;

        private List<AudioClip> BGMClips;

        private bool waiting = false;
        private void Awake()
        {
            if (bgmManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(bgmManager.gameObject);
            }
            bgmManager = this;
            DontDestroyOnLoad(this.gameObject);
            bgm.volume = standVol;
            StartCoroutine(RunSongs());
        }

        private IEnumerator RunSongs()
        {
            while (true)
            {
                if (!bgm.isPlaying && !waiting && BGMClips is {Count: > 0})
                {
                    PlayNewSong();
                }
                yield return new WaitForSeconds(0);
            }
        }

        public void SetBGM(List<AudioClip> clips)
        {
            BGMClips = clips;
            if(BGMClips.Count > 0)
                PlayNewSong();
        }

        private void PlayNewSong()
        {
            bgm.Stop();
            bgm.clip = BGMClips[Random.Range(0, BGMClips.Count)];
            bgm.Play();
        }

        public void Pause()
        {
            bgm.Pause();
            waiting = true;
        }

        public void Resume()
        {
            bgm.Play();
            waiting = false;
        }

        public IEnumerator StopBGMForTime(float time)
        {
            bgm.Pause();
            waiting = true;
            yield return new WaitForSeconds(time);
            waiting = false;
            bgm.Play();
        }

        public IEnumerator QuietBGMForTime(float time)
        {
            bgm.volume = quietVol;
            waiting = true;
            yield return new WaitForSeconds(time);
            waiting = false;
            bgm.volume = standVol;
        }

        public IEnumerator StopBGMUntilDone(AudioSource running)
        {
            bgm.Pause();
            waiting = true;
            yield return new WaitForSeconds(0);
            while (true)
            {
                if (!running.isPlaying)
                    break;
                yield return new WaitForSeconds(0);
            }
            waiting = false;
            bgm.Play();
        }
        
        public IEnumerator QuietBGMUntilDone(AudioSource running)
        {
            bgm.volume = quietVol;
            waiting = true;
            yield return new WaitForSeconds(0);
            while (true)
            {
                if (!running.isPlaying)
                    break;
                yield return new WaitForSeconds(0);
            }
            waiting = false;
            bgm.volume = standVol;
        }
    }
}
