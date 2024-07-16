using System;
using UnityEngine;

namespace Narration.Triggers
{
    public class PleistoceneLoad1 : PlayOnLoad
    {
        public Narration wallHit;

        protected override void Start()
        {
            if (!clip.GetPlayability()) return;
            wallHit.SetPlayability(true);
            NarrationManager.narrationManager.SetPlayability("FQ7", true);
            base.Start();
        }
    }
}
