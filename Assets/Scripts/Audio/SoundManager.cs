using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Audio
{
    //manages cycling through background music tracks and quieting them when other sounds need to play
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager soundManager;

        [Tooltip("Ref to BGM audio source")] public AudioSource bgm;

        [Tooltip("Standard volume of sounds")] public float standVol;

        [Tooltip("Reference to the master mixer")] public AudioMixer mainMixer;

        [Tooltip("name of settings file to save to")] public String fileName;

        [Tooltip("names of the sound parameters, in the order master volume, music volume, effect volume")]
        public List<String> volParams;

        //the slider values for the player's audio preferences
        //In order: master, music, effects
        private List<float> sliderVals;

        //list of audio clips to draw from when selecting a new track
        private List<AudioClip> BGMClips;

        //whether the bgm manager should wait to play a new song
        private bool waiting = false;
        
        //default value to quiet to when quieting bgm
        private const float quietVol = 0.2f;
        
        //set up singleton and start corountines
        private void Awake()
        {
            if (soundManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(soundManager.gameObject);
            }
            soundManager = this;
            DontDestroyOnLoad(this.gameObject);
            //set relative volume of the bgm. is not on log scale
            bgm.volume = standVol;
            try
            {
                sliderVals = JsonSerializer.Deserialize<List<float>>(File.ReadAllText("Saves/"+fileName+".json"));
            }
            catch (IOException){ }
            sliderVals ??= new List<float>();
            while (sliderVals.Count < volParams.Count)
            {
                sliderVals.Add(standVol);
            }
            StartCoroutine(RunSongs());
        }

        private void Start()
        {
            for (int i = 0; i < volParams.Count; i++)
            {
                mainMixer.SetFloat(volParams[i], ConvertToLogScale(sliderVals[i]));
            }
        }

        private void OnEnable()
        {
            
        }

        //save sound settings
        private void OnDisable()
        {
            string completedJson = JsonSerializer.Serialize(sliderVals);
            Directory.CreateDirectory("Saves");
            File.WriteAllText("Saves/"+fileName+".json", completedJson);
        }

        //check each frame if a new song should be started and start it if so
        private IEnumerator RunSongs()
        {
            while (true)
            {
                if (!bgm.isPlaying && !AudioListener.pause && !waiting && BGMClips is {Count: > 0})
                {
                    PlayNewSong();
                }
                yield return new WaitForSeconds(0);
            }
        }
        
        //check if the audio from a particular source is muted
        public bool IsMuted(int index)
        {
            return (sliderVals[index] < 0.01f || sliderVals[0] < 0.01f);
        }
        
        //change the volume of the parameter at the passed index (0 for master, 1 for music, 2 for effects)
        public void SetVol(int index, float vol)
        {
            sliderVals[index] = vol;
            mainMixer.SetFloat(volParams[index], ConvertToLogScale(sliderVals[index]));
        }

        public float GetVol(int index)
        {
            return sliderVals[index];
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
        public IEnumerator QuietBGMForTime(float time, float attenuationFactor = quietVol)
        {
            bgm.volume *= attenuationFactor;
            waiting = true;
            yield return new WaitForSeconds(time);
            waiting = false;
            bgm.volume = bgm.volume/attenuationFactor;
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
        public IEnumerator QuietBGMUntilDone(AudioSource running, float attenuationFactor = quietVol)
        {
            bgm.volume *= attenuationFactor;
            waiting = true;
            yield return new WaitForSeconds(0);
            while (true)
            {
                if (!running.isPlaying)
                    break;
                yield return new WaitForSeconds(0);
            }
            waiting = false;
            bgm.volume = bgm.volume/attenuationFactor;
        }

        private float ConvertToLogScale(float input)
        {
            return Mathf.Log10(input) * 20;
        }
    }
}
