using System;
using Audio;
using QuestSystem;
using ScriptTags;
using UnityEngine;

namespace Narration.Triggers
{
    public class PlayOnTriggerEnter : MonoBehaviour
    {
        public Narration clip;

        public bool disablePlayability = true;
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>() != null && clip.GetPlayability())
            {
                clip.Begin();
                clip.SetPlayability(!disablePlayability);
            }
        }
    }
}
