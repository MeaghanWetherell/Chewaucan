using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Events;

namespace Narration
{
    [CreateAssetMenu(menuName = "Narration/basic")]
    public class Narration : ScriptableObject
    {
        public AudioClip narrationClip;

        private List<UnityAction<string>> defaultOnComplete;

        public virtual void Begin()
        {
            defaultOnComplete ??= new List<UnityAction<string>>();
            SoundManager.soundManager.PlayNarration(narrationClip, defaultOnComplete);
            NarrationManager.narrationManager.Played(name);
        }

        public virtual void Begin(List<UnityAction<string>> onComplete)
        {
            addToOnComplete(onComplete);
            SoundManager.soundManager.PlayNarration(narrationClip, defaultOnComplete);
            NarrationManager.narrationManager.Played(name);
        }

        public void addToOnComplete(List<UnityAction<string>> onComplete)
        {
            defaultOnComplete ??= new List<UnityAction<string>>();
            foreach (UnityAction<string> action in onComplete)
            {
                if(!defaultOnComplete.Contains(action))
                    defaultOnComplete.Add(action);
            }
        }

        public void ResetOnComplete()
        {
            defaultOnComplete = new List<UnityAction<string>>();
        }

        public bool GetPlayability()
        {
            return NarrationManager.narrationManager.ShouldPlay(name);
        }

        public void SetPlayability(bool set)
        {
            NarrationManager.narrationManager.SetPlayability(name, set);
        }
    }
}
