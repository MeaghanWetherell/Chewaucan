using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Misc;
using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Audio
{
    //manages all audio
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager soundManager;

        [Tooltip("Prefab reference to the subtitle shower")] public GameObject subtitlePrefab;

        [Tooltip("Ref to BGM audio source")] public AudioSource bgm;

        [Tooltip("Ref to narration audio source")] public AudioSource narrator;

        [Tooltip("Standard volume of sounds")] public float standVol;

        [Tooltip("Reference to the master mixer")] public AudioMixer mainMixer;

        [Tooltip("name of settings file to save to")] public String fileName;

        [Tooltip("names of the sound parameters, in the order master volume, narration volume, music volume, effect volume. Must match names of exposed parameters in master mixer")]
        public List<String> volParams;

        [NonSerialized] public bool subtitlesOn;

        //the slider values for the player's audio preferences
        //In order: master, narration, music, effects
        private List<float> sliderVals;

        //list of audio clips to draw from when selecting a new track
        private List<AudioClip> BGMClips;

        //whether the bgm manager should wait to play a new song
        private bool waiting = false;
        
        //default value to quiet to when quieting bgm
        private const float quietVol = 0.1f;

        //actions to run after narration finishes
        private UnityEvent<string> onNarrationComplete;

        private List<float> currentSubTimes;

        private List<string> currentSubLines;

        private TextMeshProUGUI subtitleViewer;

        //whether the most recent narration finished
        private bool narrFinished = true;

        private Coroutine waitforcomp;

        private Coroutine runSubs;

        public void ClearSoundManager()
        {
            if(waitforcomp != null) StopCoroutine(waitforcomp);
            if(runSubs != null)StopCoroutine(runSubs);
            narrator.Stop();
            narrFinished = true;
            onNarrationComplete = null;
            currentSubTimes = null;
            currentSubLines = null;
        }

        //plays a clip through the narration source
        //will run any actions in onComplete after the narration finishes
        //including if the narration was interrupted by skipping
        public void PlayNarration(AudioClip clip, UnityEvent<string> onComplete, bool skippable = true, List<float> times = null, List<string> lines = null)
        {
            narrator.Stop();
            narrator.clip = clip;
            onNarrationComplete = onComplete;
            GameObject temp = Subtitler.subtitler;
            if (HUDManager.hudManager != null)
            {
                if (HUDManager.hudManager.skipBG != null)
                {
                    HUDManager.hudManager.skipBG.SetActive(skippable);
                }
            }
            if (subtitlesOn && times != null && lines != null && temp != null)
            {
                currentSubLines = lines;
                currentSubTimes = times;
                if (HUDManager.hudManager != null)
                {
                    if (HUDManager.hudManager.subtitleBG != null)
                    {
                        HUDManager.hudManager.subtitleBG.SetActive(true);
                    }
                }
                subtitleViewer = temp.GetComponent<TextMeshProUGUI>();
                runSubs = StartCoroutine(RunSubtitles());
            }
            PlayNarration();
            if(waitforcomp != null) StopCoroutine(waitforcomp);
            waitforcomp = StartCoroutine(WaitForNarrationComplete());
        }

        //plays the currently set narration
        public void PlayNarration()
        {
            if(!narrFinished)
                onNarrationComplete.Invoke(narrator.clip.name);
            narrFinished = false;
            narrator.Play();
            waiting = false;
            StartCoroutine(QuietBGMUntilDone(narrator));
            StartCoroutine(QuietSEUntilDone(narrator));
        }

        private void OnApplicationQuit()
        {
            StopNarration();
        }

        //stop the active narration
        public void StopNarration()
        {
            if (narrator.isPlaying)
            {
                InvokeNarrComplete();
                narrator.Stop();
            }
        }
        
        //pause the active narration
        public void PauseNarration()
        {
            narrator.Pause();
            waiting = true;
        }

        //pause/resume subtitles
        private void SubtitlePause()
        {
            if (subtitleViewer != null)
            {
                subtitleViewer.transform.parent.parent.gameObject.SetActive(false);
            }
        }
        private void SubtitleResume()
        {
            if (subtitleViewer != null)
            {
                subtitleViewer.transform.parent.parent.gameObject.SetActive(true);
            }
        }

        //invoke all actions that need to be invoked on narration complete
        private void InvokeNarrComplete()
        {
            StopCoroutine(waitforcomp);
            if (HUDManager.hudManager != null)
            {
                if (HUDManager.hudManager.skipBG != null)
                {
                    HUDManager.hudManager.skipBG.SetActive(false);
                }
            }
            narrFinished = true;
            if(runSubs != null)StopCoroutine(runSubs);
            if(subtitleViewer != null)Destroy(subtitleViewer.transform.parent.parent.gameObject);
            currentSubLines = null;
            currentSubTimes = null;
            onNarrationComplete.Invoke(narrator.clip.name);
        }

        private IEnumerator RunSubtitles()
        {
            subtitleViewer.text = currentSubLines[0];
            yield return new WaitForSeconds(currentSubTimes[0]);
            int i = 1;
            while (currentSubLines != null && currentSubTimes != null && i < currentSubLines.Count && subtitleViewer != null)
            {
                while (AudioListener.pause || !subtitleViewer.transform.parent.gameObject.activeSelf) yield return new WaitForSeconds(0);
                subtitleViewer.text = currentSubLines[i];
                yield return new WaitForSeconds(currentSubTimes[i] - currentSubTimes[i - 1]);
                i++;
            }
            subtitleViewer.text = "";
        }

        //checks every frame to see if narration has finished
        private IEnumerator WaitForNarrationComplete()
        {
            while (true)
            {
                //Debug.Log(narrator.isPlaying);
                //Debug.Log(AudioListener.pause);
                //Debug.Log(waiting);
                //Debug.Log(narrFinished);
                if (!narrator.isPlaying && !AudioListener.pause && !waiting && !narrFinished)
                {
                    InvokeNarrComplete();
                }
                yield return new WaitForSeconds(0);
            }
        }
        
        //checks every frame to see if narration has finished
        private IEnumerator WaitForNarrationComplete(List<float> times, List<string> lines, TextMeshProUGUI text)
        {
            while (true)
            {
                if (!narrator.isPlaying && !AudioListener.pause && !PauseCallback.pauseManager.isPaused && !waiting && !narrFinished)
                {
                    InvokeNarrComplete();
                }
                yield return new WaitForSeconds(0);
            }
        }
        
        //set up singleton and start corountines
        private void Awake()
        {
            //set relative volume of the bgm and narrator. is not on log scale
            bgm.volume = standVol;
            narrator.volume = standVol;
            sliderVals ??= new List<float>();
            while (sliderVals.Count < volParams.Count)
            {
                sliderVals.Add(standVol);
            }
            StartCoroutine(RunSongs());
            if (soundManager != null)
            {
                Destroy(gameObject);
                return;
            }
            soundManager = this;
            DontDestroyOnLoad(this.gameObject);
            SaveHandler.saveHandler.subSettingToSave(Save);
            SaveHandler.saveHandler.subSettingToLoad(Load);
        }

        //set the mixer values. I can't remember *why* this needs to be in start instead of awake, but it does
        private void Start()
        {
            for (int i = 0; i < volParams.Count; i++)
            {
                mainMixer.SetFloat(volParams[i], ConvertToLogScale(sliderVals[i]));
            }
            PauseCallback.pauseManager.SubscribeToPause(SubtitlePause);
            PauseCallback.pauseManager.SubscribeToResume(SubtitleResume);
        }

        private void Load(string path)
        {
            StopAllCoroutines();
            narrator.clip = null;
            StartCoroutine(RunSongs());
            try
            {
                sliderVals = JsonSerializer.Deserialize<List<float>>(File.ReadAllText(path+"/"+fileName+".json"));
                subtitlesOn = JsonSerializer.Deserialize<bool>(File.ReadAllText(path+"/subtitlesOn.json"));
            }
            catch (IOException){ }
            sliderVals ??= new List<float>();
            while (sliderVals.Count < volParams.Count)
            {
                sliderVals.Add(standVol);
            }
            for (int i = 0; i < volParams.Count; i++)
            {
                mainMixer.SetFloat(volParams[i], ConvertToLogScale(sliderVals[i]));
            }
        }

        private void Save(string path)
        {
            StopAllCoroutines();
            narrator.clip = null;
            StartCoroutine(RunSongs());
            string completedJson = JsonSerializer.Serialize(sliderVals);
            File.WriteAllText(path+"/"+fileName+".json", completedJson);
            string subsOn = JsonSerializer.Serialize(subtitlesOn);
            File.WriteAllText(path + "/subtitlesOn.json", subsOn);
        }

        //save sound settings
        private void OnDisable()
        {
            PauseCallback.pauseManager.UnsubToPause(SubtitlePause);
            PauseCallback.pauseManager.UnsubToResume(SubtitleResume);
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
        
        //change the volume of the parameter at the passed index (0 for master, 1 for music, 2 for effects, 3 for narration)
        public void SetVol(int index, float vol)
        {
            sliderVals[index] = vol;
            mainMixer.SetFloat(volParams[index], ConvertToLogScale(sliderVals[index]));
        }

        //get the volume of the parameter at the passed index (0 for master, 1 for music, 2 for effects, 3 for narration)
        public float GetVol(int index)
        {
            return sliderVals[index];
        }

        //set the track list for the current area's background music
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
                if (!running.isPlaying && !AudioListener.pause)
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
                if (!running.isPlaying && !AudioListener.pause)
                    break;
                yield return new WaitForSeconds(0);
            }
            waiting = false;
            bgm.volume = bgm.volume/attenuationFactor;
        }
        
        //quiets SE by the set factor until the passed audio source stops playing
        public IEnumerator QuietSEUntilDone(AudioSource running, float attenuationFactor = quietVol)
        {
            float curVol;
            mainMixer.GetFloat("EffectVol", out curVol);
            float changeVol = InverseLogScale(curVol) * attenuationFactor;
            mainMixer.SetFloat("EffectVol", ConvertToLogScale(changeVol));
            waiting = true;
            yield return new WaitForSeconds(0);
            while (true)
            {
                if (!running.isPlaying && !AudioListener.pause)
                    break;
                yield return new WaitForSeconds(0);
            }
            waiting = false;
            mainMixer.SetFloat("EffectVol", curVol);
        }

        //reverses conversion to log scale, returning to linear
        private float InverseLogScale(float input)
        {
            return Mathf.Pow(10, ((input-10) / 20));
        }

        //audio mixer volumes are in decibels, which are on a logarithmic scale rather than a linear one
        //this converts a linear percentage volume into one that works for a log scale
        private float ConvertToLogScale(float input)
        {
            return Mathf.Log10(input) * 20+10;
        }
    }
}
