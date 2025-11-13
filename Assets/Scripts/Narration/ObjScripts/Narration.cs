using System;
using System.Collections.Generic;
using Audio;
using KeyRebinding;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Narration
{
    [CreateAssetMenu(menuName = "Narration/basic")]
    //scriptable object to store narration details. Recommend extending to add additional actions on narration complete
    //serialized objects are weird, I believe it will deallocate the memory for the onComplete list on termination,
    //but if there's a leak check here
    public class Narration : ScriptableObject
    {
        //clip associated with this narration
        public AudioClip narrationClip;
        
        //the subtitle doc for this narration
        public TextAsset subtitles;

        //whether this narration should be playable by default
        //disable this is if it should be unavailable until a certain action has been performed,
        //otherwise enable. playability is not automatically used by the narration,
        //but may be checked in code to determine if narration should play
        public bool defaultPlayability;

        //runs when narration finishes
        private UnityEvent<string> narrCompleted;

        //the prefab prompt allowing the player to skip narration
        private static GameObject narrSkipPrompt;
        
        //start the narration
        public virtual void Begin(bool skippable = true)
        {
            Begin(new List<UnityAction<string>>(), skippable);
        }

        public virtual void Stop()
        {
            if (SoundManager.soundManager != null && SoundManager.soundManager.narrator.clip != null)
            {
                if (SoundManager.soundManager.narrator.clip.Equals(narrationClip))
                {
                    SoundManager.soundManager.StopNarration();
                }
            }
        }
        
        //start the narration, running any actions in the passed list when the narration finishes
        public virtual void Begin(List<UnityAction<string>> onComplete, bool skippable = true)
        {
            addToOnComplete(onComplete);
            if (subtitles != null)
            {
                (List<float>, List<string>) subs = parseSubtitles();
                SoundManager.soundManager.PlayNarration(narrationClip, narrCompleted, skippable, subs.Item1, subs.Item2);
            }
            else SoundManager.soundManager.PlayNarration(narrationClip, narrCompleted, skippable);
            NarrationManager.narrationManager.Played(name);
        }

        //adds an item to this narration's onComplete list
        public void addToOnComplete(List<UnityAction<string>> onComplete)
        {
            narrCompleted ??= new UnityEvent<string>();
            foreach (UnityAction<string> action in onComplete)
            {
                narrCompleted.AddListener(action);
            }
        }

        //remove functions from the oncomplete callback
        public void RemoveFromOnComplete(List<UnityAction<string>> onComplete)
        {
            foreach (UnityAction<string> action in onComplete)
            {
                narrCompleted.RemoveListener(action);
            }
        }

        //resets this Narration's onComplete list
        public void ResetOnComplete()
        {
            narrCompleted = new UnityEvent<string>();
        }

        //return whether this narration has been played or not
        public bool HasPlayed()
        {
            return NarrationManager.narrationManager.HasPlayed(name);
        }

        //determine whether this narration should be played
        public bool GetPlayability()
        {
            if(NarrationManager.narrationManager.HasDataOnNarr(name))
                return NarrationManager.narrationManager.ShouldPlay(name);
            return defaultPlayability;
        }

        //set whether this Narration should play
        public void SetPlayability(bool set)
        {
            NarrationManager.narrationManager.SetPlayability(name, set);
        }

        private (List<float>, List<string>) parseSubtitles()
        {
            List<float> times = new List<float>();
            List<string> lines = new List<string>();
            string fileText = subtitles.ToString();
            string[] textByLine = fileText.Split('\n');
            for (int i = 0; i < textByLine.Length; i++)
            {
                textByLine[i] = textByLine[i].Trim(new Char[] {'\r'});
            }
            for (int i = 0; i < textByLine.Length; i++)
            {
                string[] cur = textByLine[i].Split('|');
                if (cur[0][0] != '#')
                {
                    float time = stringTimeToFloat(cur[0]);
                    times.Add(time);
                    lines.Add(cur[1]);
                }
            }
            return (times, lines);
        }

        private float stringTimeToFloat(string conv)
        {
            string[] cur = conv.Split(':');
            float ret = 0;
            ret += float.Parse(cur[0]) * 60;
            ret += float.Parse(cur[1]);
            return ret;
        }
    }
}
