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

        //the onComplete list that actually gets passed to the sound manager
        private List<UnityAction<string>> actualOnComplete;

        //the prefab prompt allowing the player to skip narration
        private static GameObject narrSkipPrompt;

        //start the narration
        public virtual void Begin()
        {
            Begin(new List<UnityAction<string>>());
        }
        
        //start the narration, running any actions in the passed list when the narration finishes
        public virtual void Begin(List<UnityAction<string>> onComplete)
        {
            addToOnComplete(onComplete);
            InstantiateSkipNarr();
            SoundManager.soundManager.PlayNarration(narrationClip, actualOnComplete);
            NarrationManager.narrationManager.Played(name);
        }

        //adds an item to this narration's onComplete list
        public void addToOnComplete(List<UnityAction<string>> onComplete)
        {
            actualOnComplete ??= new List<UnityAction<string>>(){DestroySkip};
            foreach (UnityAction<string> action in onComplete)
            {
                if(!actualOnComplete.Contains(action))
                    actualOnComplete.Add(action);
            }
        }

        //resets this Narration's onComplete list
        public void ResetOnComplete()
        {
            actualOnComplete = new List<UnityAction<string>>();
        }

        //determine whether this narration should be played
        public bool GetPlayability()
        {
            return NarrationManager.narrationManager.ShouldPlay(name);
        }

        //set whether this Narration should play
        public void SetPlayability(bool set)
        {
            NarrationManager.narrationManager.SetPlayability(name, set);
        }

        //after narration has finished playing, destroy the skip prompt
        private static void DestroySkip(string doesntmatter)
        {
            Destroy(GameObject.Find("SkipNarr(Clone)"));
        }

        //when narration starts, instantiate the prompt to skip it
        private static void InstantiateSkipNarr()
        {
            narrSkipPrompt ??= Resources.Load<GameObject>("SkipNarr");
            Instantiate(narrSkipPrompt);
        }
    }
}
