using System;
using UnityEngine;

namespace Narration.Triggers
{
    //if the clip is marked playable, play it automatically on scene load
    public class PlayOnLoad : MonoBehaviour
    {
        [Tooltip("Clip to play automatically on scene load")]
        public Narration clip;
        
        protected virtual void Start()
        {
            if (!clip.GetPlayability()) return;
            clip.Begin();
            clip.SetPlayability(false);
        }
    }
}
