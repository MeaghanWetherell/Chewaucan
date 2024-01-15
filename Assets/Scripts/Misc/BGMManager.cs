using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
    //manages cycling through background music tracks and quieting them when other sounds need to play
    public class BGMManager : MonoBehaviour
    {
        public static BGMManager bgmManager;

        [Tooltip("Ref to BGM audio source")] public AudioSource bgm;

        [Tooltip("The volume to reduce the BGM to when quieted. must be from 0 to 1")]
        public float quietVol;

        [Tooltip("Standard volume of the BGM")] public float standVol;

        //list of audio clips to draw from when selecting a new track
        private List<AudioClip> BGMClips;

        //whether the bgm manager should wait to play a new song
        private bool waiting = false;
        
        //set up singleton and start corountines
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

        //check each frame if a new song should be started and start it if so
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

        //set the track list for the current areas background music
        public void SetBGM(List<AudioClip> clips)
        {
            BGMClips = clips;
            if(BGMClips.Count > 0)
                PlayNewSong();
        }

        //stops the bgm and entirely and resets the track list
        public void StopBGM()
        {
            BGMClips = null;
            bgm.Stop();
        }

        //plays a random song from the list
        private void PlayNewSong()
        {
            bgm.Stop();
            bgm.clip = BGMClips[Random.Range(0, BGMClips.Count)];
            bgm.Play();
        }

        //pauses the bgm
        public void Pause()
        {
            bgm.Pause();
            waiting = true;
        }

        //resumes the bgm
        public void Resume()
        {
            bgm.Play();
            waiting = false;
        }

        //stops the bgm entirely for the passed time
        public IEnumerator StopBGMForTime(float time)
        {
            bgm.Pause();
            waiting = true;
            yield return new WaitForSeconds(time);
            waiting = false;
            bgm.Play();
        }

        //quiets the bgm by the set factor for the passed time
        public IEnumerator QuietBGMForTime(float time)
        {
            bgm.volume = quietVol;
            waiting = true;
            yield return new WaitForSeconds(time);
            waiting = false;
            bgm.volume = standVol;
        }

        //stops the bgm until the passed audio source stops playing
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
        
        //quiets the bgm by the set factor until the passed audio source stops playing
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
