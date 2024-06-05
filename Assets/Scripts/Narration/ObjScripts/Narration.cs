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

        [Tooltip("IDs must be unique")]public string clipID;

        public virtual void Begin()
        {
            SoundManager.soundManager.PlayNarration(narrationClip, new List<UnityAction>());
            NarrationManager.narrationManager.Played(clipID);
        }

        public virtual void Begin(List<UnityAction> onComplete)
        {
            SoundManager.soundManager.PlayNarration(narrationClip, onComplete);
            NarrationManager.narrationManager.Played(clipID);
        }

        public bool GetPlayability()
        {
            return NarrationManager.narrationManager.ShouldPlay(clipID);
        }

        public void SetPlayability(bool set)
        {
            NarrationManager.narrationManager.SetPlayability(clipID, set);
        }
    }
}
