using System;
using Audio;
using QuestSystem;
using ScriptTags;
using UnityEngine;

namespace Narration.Triggers
{
    //play a clip on trigger enter if the clip is marked playable
    public class PlayOnTriggerEnter : MonoBehaviour
    {
        public Narration clip;

        [Tooltip("Whether to disable playability of this narration after it is played by this script")]
        public bool disablePlayability = true;
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>() != null && clip.GetPlayability())
            {
                clip.Begin();
                clip.SetPlayability(!disablePlayability);
                this.enabled = false;
            }
        }
    }
}
