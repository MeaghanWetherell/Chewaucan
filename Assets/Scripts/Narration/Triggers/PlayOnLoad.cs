using System;
using UnityEngine;

namespace Narration.Triggers
{
    public class PlayOnLoad : MonoBehaviour
    {
        public Narration clip;
        
        protected virtual void Start()
        {
            if (!clip.GetPlayability()) return;
            clip.Begin();
            clip.SetPlayability(false);
        }
    }
}
