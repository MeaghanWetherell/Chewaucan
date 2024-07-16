using System;
using System.Collections.Generic;
using Audio;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;

namespace Narration
{
    [CreateAssetMenu(menuName = "Narration/basic")]
    public class Narration : ScriptableObject
    {
        public AudioClip narrationClip;

        private List<UnityAction<string>> defaultOnComplete;

        private static GameObject narrSkipButton;

        public virtual void Begin()
        {
            Begin(new List<UnityAction<string>>());
        }

        public virtual void Begin(List<UnityAction<string>> onComplete)
        {
            addToOnComplete(onComplete);
            InstantiateSkipNarr();
            SoundManager.soundManager.PlayNarration(narrationClip, defaultOnComplete);
            NarrationManager.narrationManager.Played(name);
        }

        public void addToOnComplete(List<UnityAction<string>> onComplete)
        {
            defaultOnComplete ??= new List<UnityAction<string>>(){DestroySkip};
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

        private static void DestroySkip(string doesntmatter)
        {
            Destroy(GameObject.Find("SkipNarr(Clone)"));
        }

        private static void InstantiateSkipNarr()
        {
            narrSkipButton ??= Resources.Load<GameObject>("SkipNarr");
            Instantiate(narrSkipButton);
        }
    }
}
